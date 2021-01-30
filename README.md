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