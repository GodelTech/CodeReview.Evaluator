﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GodelTech.CodeReview.Evaluator.Models;
using Microsoft.Data.Sqlite;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class DatabaseService : IDatabaseService
    {
        internal const string IssuesTableName = "Issues";
        internal const string FileDetailsTableName = "FileDetails";
        
        private const string ColumnNameField = "ColumnName";
        private const string ParameterNamePrefix = "$";

        private static readonly Dictionary<string, string> EmptyDictionary = new();


        private readonly IInitScriptProvider _scriptProvider;

        public DatabaseService(IInitScriptProvider scriptProvider)
        {
            _scriptProvider = scriptProvider ?? throw new ArgumentNullException(nameof(scriptProvider));
        }

        public async Task<bool> DoesTableExist(string dbFilePath, string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tableName));
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));

            var result = await ExecuteScalarAsync(dbFilePath,
                @"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=$TableName",
                new Dictionary<string, object>
                {
                    ["TableName"] = tableName
                });

            return (long)result > 0;
        }

        public async Task CreateFileDetailsDbAsync(string dbFilePath)
        {
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));

            await ExecuteNonQueryAsync(dbFilePath, _scriptProvider.GetFileDetailsDbScript(), new Dictionary<string, object>());
        }

        public async Task CreateIssuesDbAsync(string dbFilePath)
        {
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));

            await ExecuteNonQueryAsync(dbFilePath, _scriptProvider.GetIssuesDbScript(), new Dictionary<string, object>());
        }

        public async Task SaveLocDetailsAsync(string dbFilePath, FileLocDetails[] items)
        {
            if (items == null) 
                throw new ArgumentNullException(nameof(items));
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));

            await using var connection = new SqliteConnection(BuildConnectionString(dbFilePath));

            await connection.OpenAsync();

            // This action is required to speed up insert operation
            // https://stackoverflow.com/questions/3852068/sqlite-insert-very-slow
            await BeginTransactionAsync(connection);

            foreach (var item in items)
            {
                await SaveLocItemAsync(connection, item);
            }

            await CommitTransactionAsync(connection);
        }

        public async Task ExecuteNonQueryAsync(string dbFilePath, string queryText, IReadOnlyDictionary<string, object> parameters)
        {
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));
            if (string.IsNullOrWhiteSpace(queryText))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(queryText));
            
            await using var connection = new SqliteConnection(BuildConnectionString(dbFilePath));

            await connection.OpenAsync();

            var command = CreateCommand(queryText, parameters, connection);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<object> ExecuteScalarAsync(string dbFilePath, string queryText, IReadOnlyDictionary<string, object> parameters)
        {
            if (parameters == null) 
                throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));
            if (string.IsNullOrWhiteSpace(queryText))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(queryText));

            await using var connection = new SqliteConnection(BuildConnectionString(dbFilePath));

            await connection.OpenAsync();

            var command = CreateCommand(queryText, parameters, connection);

            var value = await command.ExecuteScalarAsync();

            return value == DBNull.Value ? null : value;
        }

        public async Task<object> ExecuteObjectAsync(string dbFilePath, string queryText, IReadOnlyDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));
            if (string.IsNullOrWhiteSpace(queryText))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(queryText));

            await using var connection = new SqliteConnection(BuildConnectionString(dbFilePath));

            await connection.OpenAsync();

            var command = CreateCommand(queryText, parameters, connection);

            await using var reader = await command.ExecuteReaderAsync();

            return reader.Read() ? ReadObject(reader.GetSchemaTable(), reader) : null;
        }

        public async Task<object> ExecuteCollectionAsync(string dbFilePath, string queryText, IReadOnlyDictionary<string, object> parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));
            if (string.IsNullOrWhiteSpace(queryText))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(queryText));

            await using var connection = new SqliteConnection(BuildConnectionString(dbFilePath));

            await connection.OpenAsync();

            var command = CreateCommand(queryText, parameters, connection);

            await using var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return null;

            var schema = reader.GetSchemaTable();
            var results = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                results.Add(ReadObject(schema, reader));
            }

            return results;
        }

        private static Dictionary<string, object> ReadObject(DataTable schema, IDataRecord reader)
        {
            var resultObject = new Dictionary<string, object>();

            foreach (DataRow row in schema.Rows)
            {
                var columnName = (string) row[ColumnNameField];
                var columnValue = reader[columnName];

                resultObject.Add(columnName, columnValue == DBNull.Value ? null : columnValue);
            }

            return resultObject;
        }

        public async Task SaveIssuesAsync(string dbFilePath, IEnumerable<Issue> issues)
        {
            if (issues == null) 
                throw new ArgumentNullException(nameof(issues));
            if (string.IsNullOrWhiteSpace(dbFilePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(dbFilePath));
            
            await using var connection = new SqliteConnection(BuildConnectionString(dbFilePath));

            await connection.OpenAsync();

            // This action is required to speed up insert operation
            // https://stackoverflow.com/questions/3852068/sqlite-insert-very-slow
            await BeginTransactionAsync(connection);

            foreach (var issue in issues)
            {
                await SaveEntityAsync(connection, issue);
            }

            await CommitTransactionAsync(connection);
        }

        #region Helper methods

        private static SqliteCommand CreateCommand(string queryText, IReadOnlyDictionary<string, object> parameters, SqliteConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText = queryText;

            foreach (var (paramName, value) in parameters)
            {
                command.Parameters.AddWithValue(ParameterNamePrefix + paramName, value);
            }

            return command;
        }

        private static async Task BeginTransactionAsync(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "begin";
            await command.ExecuteNonQueryAsync();
        }

        private static async Task CommitTransactionAsync(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "end";
            await command.ExecuteNonQueryAsync();
        }

        private static async Task SaveEntityAsync(SqliteConnection connection, Issue issue)
        {
            var issueId = await SaveIssueAsync(connection, issue);

            foreach (var location in (issue.Locations ?? Array.Empty<IssueLocation>()).Distinct())
            {
                await SaveLocationAsync(connection, issueId, location);
            }

            foreach (var tag in issue.Tags ?? Array.Empty<string>())
            {
                await SaveTagAsync(connection, issueId, tag);
            }

            foreach (var hash in issue.Hashes ?? EmptyDictionary)
            {
                await SaveHashAsync(connection, issueId, hash.Key, hash.Value);
            }

            foreach (var property in issue.Properties ?? EmptyDictionary)
            {
                await SavePropertyAsync(connection, issueId, property.Key, property.Value);
            }
        }

        private static async Task SavePropertyAsync(SqliteConnection connection, long issueId, string name, string value)
        {
            var issueLocationCommand = connection.CreateCommand();

            issueLocationCommand.CommandText = @"INSERT INTO IssueProperties 
                (
	                IssueId, 
	                Property,
                    Value
                )
                VALUES (
                    $issueId,	
                    $property,
                    $value
                );";

            issueLocationCommand.Parameters.AddWithValue("$issueId", issueId);
            issueLocationCommand.Parameters.AddWithValue("$property", name);
            issueLocationCommand.Parameters.AddWithValue("$value", value);

            await issueLocationCommand.ExecuteNonQueryAsync();
        }

        private static async Task SaveLocItemAsync(SqliteConnection connection, FileLocDetails details)
        {
            var issueCommand = connection.CreateCommand();

            issueCommand.CommandText = @"INSERT INTO FileDetails 
            (
	            FilePath,
	            Language,
	            Blank,
	            Code,
	            Commented
            )
            VALUES (
                $filePath,
                $language,
                $blank,
                $code,
                $commented
            );";

            issueCommand.Parameters.AddWithValue("$filePath", details.FilePath);
            issueCommand.Parameters.AddWithValue("$language", details.Language ?? string.Empty);
            issueCommand.Parameters.AddWithValue("$blank", details.Blank);
            issueCommand.Parameters.AddWithValue("$code", details.Code);
            issueCommand.Parameters.AddWithValue("$commented", details.Commented);

            await issueCommand.ExecuteNonQueryAsync();
        }

        private static async Task<long> SaveIssueAsync(SqliteConnection connection, Issue issue)
        {
            var issueCommand = connection.CreateCommand();

            issueCommand.CommandText = @"INSERT INTO Issues 
            (
	            RuleId,
	            Level,
                Title,
	            Message,
	            Description,
	            DetailsUrl,
	            Category
            )
            VALUES (
                $ruleId,
                $level,
                $title,
                $message,
                $description,
                $detailsUrl,
                $category
            );";

            issueCommand.Parameters.AddWithValue("$ruleId", issue.RuleId);
            issueCommand.Parameters.AddWithValue("$level", issue.Level);
            issueCommand.Parameters.AddWithValue("$title", (object)issue.Title ?? DBNull.Value);
            issueCommand.Parameters.AddWithValue("$message", issue.Message);
            issueCommand.Parameters.AddWithValue("$description", (object) issue.Description ?? DBNull.Value);
            issueCommand.Parameters.AddWithValue("$detailsUrl", (object) issue.DetailsUrl ?? DBNull.Value);
            issueCommand.Parameters.AddWithValue("$category", (object) issue.Category ?? DBNull.Value);

            await issueCommand.ExecuteNonQueryAsync();


            var identityCommand = connection.CreateCommand();

            identityCommand.CommandText = "SELECT last_insert_rowid()";
            
            var issueId = await identityCommand.ExecuteScalarAsync();

            return (long)issueId;
        }

        private static async Task SaveLocationAsync(SqliteConnection connection, long issueId, IssueLocation location)
        {
            var issueLocationCommand = connection.CreateCommand();

            issueLocationCommand.CommandText = @"INSERT INTO IssueLocations 
                (
	                IssueId, 
	                FilePath, 
                    StartLine, 
	                EndLine 
                )
                VALUES (
                    $issueId,	
                    $filePath,	
                    $startLine,	
                    $endLine	
                );";

            issueLocationCommand.Parameters.AddWithValue("$issueId", issueId);
            issueLocationCommand.Parameters.AddWithValue("$filePath", location.FilePath);
            issueLocationCommand.Parameters.AddWithValue("$startLine", (object) location.Region?.StartLine ?? DBNull.Value);
            issueLocationCommand.Parameters.AddWithValue("$endLine", (object) location.Region?.EndLine ?? DBNull.Value);

            await issueLocationCommand.ExecuteNonQueryAsync();
        }

        private static async Task SaveTagAsync(SqliteConnection connection, long issueId, string tag)
        {
            var issueLocationCommand = connection.CreateCommand();

            issueLocationCommand.CommandText = @"INSERT INTO IssueTags 
                (
	                IssueId, 
	                Name 
                )
                VALUES (
                    $issueId,	
                    $name	
                );";

            issueLocationCommand.Parameters.AddWithValue("$issueId", issueId);
            issueLocationCommand.Parameters.AddWithValue("$name", tag);

            await issueLocationCommand.ExecuteNonQueryAsync();
        }

        private static async Task SaveHashAsync(SqliteConnection connection, long issueId, string algorithmName, string hashValue)
        {
            var issueLocationCommand = connection.CreateCommand();

            issueLocationCommand.CommandText = @"INSERT INTO IssueHashes 
                (
	                IssueId, 
	                Algorithm,
                    Value
                )
                VALUES (
                    $issueId,	
                    $algorithm,
                    $value
                );";

            issueLocationCommand.Parameters.AddWithValue("$issueId", issueId);
            issueLocationCommand.Parameters.AddWithValue("$algorithm", algorithmName);
            issueLocationCommand.Parameters.AddWithValue("$value", hashValue);

            await issueLocationCommand.ExecuteNonQueryAsync();
        }

        private static string BuildConnectionString(string filePath)
        {
            return "Data Source=" + filePath + "; Pooling=False;";
        }

        #endregion
    }
}
