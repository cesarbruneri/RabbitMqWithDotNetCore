# RabbitMqWithDotNetCore
Projeto p/ entender funcionamento de serviços de mensageria.

# Tecnologias
* .Net Core
* Docker
* RabbitMq

# Inicialização
1 - Realizar o clone do projeto com git clone.

2 - Inicializar o docker.

3 - Executar o comando:
```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management
```

4 - abrir o projeto no VSCode ou Visual Studio.

5 - Executar o projeto Send p/ Enviar dados p/ o servidor rabbitMQ no docker.

6 - Executar o projeto Receive p/ consumir mensagens do servidor rabbitMQ no docker.
