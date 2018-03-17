# CoreMicroservicesSample
.Net Core Microservices Architecture using RabbitMQ

## Gateway (API)
.net Core http client

## Common
Shared library between services

## Activities/Identity
Micro services

## Prerequistes
- Visual Studio 2017 (or equivalent)
- .Net Core SDK 2.0
- RabbitMq running (use CloudAMQP or docker "rabbitmq")
- Mongodb running (default to dockerized localhost "mongo")

## Application secrets
For mongodb and rabbitmq, fill theses configuration variables :
```
{
  "mongodb_connectionstring": "mongodb://<username>:<password>@<hostname>:<port>[/<optional_databasename>]",
  "rabbitmq_connectionstring":  "<username>:<password>@<hostname>/<route>" 
}
```

## Credits/Tools
- Docker : https://www.docker.com/
- CloudAMQP : https://www.cloudamqp.com/
- Packt Publishing : https://www.udemy.com/net-core-microservices
- Kitematic (docker GUI) : https://github.com/docker/kitematic/releases
- Robo 3T (Mongodb client) : https://robomongo.org/
- Postman : https://www.getpostman.com/
- mLab : https://mlab.com
