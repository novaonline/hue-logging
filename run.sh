#!/bin/bash

streamName="hue-logging-stream"
streamVersion="1.0-SNAPSHOT"
streamBasePath="stream/${streamName}"
streamJarPath="${streamBasePath}/target/${streamName}-${streamVersion}.jar"

# Before running this, please make sure that you have a .env file and the expected variables exist. See sample.env for expected variables
# TODO fix the hostname issue. Services should be able to run and communicate using hostname/alias

# First we need to build stream using Maven
echo "Building Stream Job"
mvn -f "${streamBasePath}/pom.xml" clean package 
if [ $? -eq 0 ] 
   # Now we need to spawn up all services
   then
    echo "Spawning up all services" 
    docker-compose up -d 
fi
if [ $? -eq 0 ]
   then 
    # Then we start the stream job
    echo "Starting Stream Job"
    JOBMANAGER_CONTAINER=$(docker ps --filter name=jobmanager --format={{.ID}})
    docker cp ${streamJarPath} "$JOBMANAGER_CONTAINER":/job.jar
    CASSANDRA_HOST_IP=`docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $(docker ps --filter name=cassandra --format={{.ID}})`
    docker exec -it -d "$JOBMANAGER_CONTAINER" flink run /job.jar --cassandraHost `echo $CASSANDRA_HOST_IP`
fi