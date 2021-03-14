# Introduction 

#### evaluate
Create issue summary using provided manifest
<pre>
> dotnet CodeReview.Evaluator.dll evaluate -m manifest.yml -r false -o result.json
</pre>
| Agruments     | Key       | Required                          | Type      | Description agrument      |
| ------------- | --------- | --------------------------------- | --------- | ------------------------- |
| --manifest    | -m        | true                              | string    | Manifest file path        |
| --loc         | -l        | false                             | string    | Lines of code statistics  |
| --folder      | -f        | false                             | string    | Path to folder or file to process  |
| --pattern     | -p        | false; "*" by defalut             | string    | Search pattern used to look for files within folder  |
| --recurse     | -r        | true; true by defalut             | bool      | Specifies if recurse search must be used for for files in folder  |
| --scope       | -s        | false                             | string    | Scope of issue to analyze |
| --output      | -o        | true                              | string    | Output file path |

#### export-db
Create issue summary and export db with result
<pre>
> dotnet CodeReview.Evaluator.dll export-db -r false -o result.db
</pre>
| Agruments     | Key       | Required                          | Type      | Description agrument      |
| ------------- | --------- | --------------------------------- | --------- | ------------------------- |
| --loc         | -l        | false                             | string    | Lines of code statistics  |
| --folder      | -f        | false                             | string    | Path to folder or file to process  |
| --pattern     | -p        | false; "*" by defalut             | string    | Search pattern used to look for files within folder  |
| --recurse     | -r        | true; true by defalut             | bool      | Specifies if recurse search must be used for for files in folder  |
| --scope       | -s        | false                             | string    | Scope of issue to analyze |
| --output      | -o        | true                              | string    | Output file path |


#### new-manifest
Creates new manifest which can be used as draft manifest
<pre>
> dotnet CodeReview.Evaluator.dll new-manifest -o output.yml
</pre>
| Agruments     | Key       | Required                          | Type      | Description agrument      |
| ------------- | --------- | --------------------------------- | --------- | ------------------------- |
| --output      | -o        | true                              | string    | Output file path          |

#### new-filter
Creates new filter which can be used as draft for real filter
<pre>
> dotnet CodeReview.Evaluator.dll new-filter -o output.yml
</pre>
| Agruments     | Key       | Required                          | Type      | Description agrument      |
| ------------- | --------- | --------------------------------- | --------- | ------------------------- |
| --output      | -o        | true                              | string    | Output file path          |