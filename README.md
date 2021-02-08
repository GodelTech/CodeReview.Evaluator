# Introduction 

How to build image 

docker build -t godeltech/codereview.evaluator:0.0.1 -f src/CodeReview.Evaluator/Dockerfile ./src
docker image tag godeltech/codereview.evaluator:0.0.1 godeltech/codereview.evaluator:latest
docker push godeltech/codereview.evaluator:latest
docker push godeltech/codereview.evaluator:0.0.1

Run:

docker run -v "/d/temp:/result"   --rm godeltech/codereview.evaluator  run -p SonarAnalyzer.CSharp -o /result/result.yaml -j

Debug:

docker run -v "/d/temp:/result" -it --rm  --entrypoint /bin/bash  godeltech/codereview.evaluator

The following tool can be used to calculate line count:

[Cloc](https://github.com/AlDanial/cloc)

Command line to execute
```bash
docker run --rm -v $PWD:/tmp aldanial/cloc . --by-file-by-lang --report-file=report.yaml --yaml
```
Example of report:

```yaml
---
# github.com/AlDanial/cloc
header : 
  cloc_url           : github.com/AlDanial/cloc
  cloc_version       : 1.88
  elapsed_seconds    : 10.9346380233765
  n_files            : 155
  n_lines            : 80124
  files_per_second   : 14.1751377291718
  lines_per_second   : 7327.54022846554
  report_file        : report.yaml
'./test/GodelTech.Microservices.Website/wwwroot/lib/bootstrap/dist/css/bootstrap.css' :
  blank: 1199
  comment: 7
  code: 8832
  language: CSS
'./test/GodelTech.Microservices.Core.Tests/obj/project.assets.json' :
  blank: 0
  comment: 0
  code: 7705
  language: JSON
```

Installing `cloc` for Ubunitu: [instructions](https://zoomadmin.com/HowToInstall/UbuntuPackage/cloc). The following commands must be used:

```bash
sudo apt-get update -y
sudo apt-get install -y cloc
```