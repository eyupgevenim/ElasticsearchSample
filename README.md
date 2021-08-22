.NET 5 Elasticsearch with VueJS Example
=======================================

Installation Guide
------------------


```shell
docker-compose up --build
```

Additional Information
----------------------

If you get this ``` ERROR: [1] bootstrap checks failed. You must address the points described in the following [1] lines before starting Elasticsearch.
...     | bootstrap check failure [1] of [1]: max virtual memory areas vm.max_map_count [65530] is too low, increase to at least [262144]```

You should look this

Set ```vm.max_map_count``` to at least ```262144``` <span style='font-size:25px;'>&#9755;</span>
[Starting a multi-node cluster with Docker Compose](https://www.elastic.co/guide/en/elasticsearch/reference/current/docker.html)

Docker for Windows, run script as administrator on powershell ->
```wsl -d docker-desktop``` 
```sysctl -w vm.max_map_count=262144```