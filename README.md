================================================================
                          Kafka Producer
================================================================
You have to install docker desktop and open docker desktop.

check the docker compose file 
you dont need zookeeper now.

and go to cmd prompt and go to the docker compose file location (cd file folder path)

and execute the below command.

docker-compose up -d 

After that go to docker desktop and open contaniners --> go to kafka UI and click on port. 

It will open http://localhost:8080/

After that write code and produce the message to kafka topic.

After run the code and check the kafka UI for the produced message.

====================================================================
                          Kafka Consumer
====================================================================

Create a worker service project and write consume code to kafka consumer
After that run both projects and produce a message and consumer will consume that message. 




