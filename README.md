# botwithflow

Welcome! 
Este chatbot esta hecho con Bot Framework SDK 4.5.3 y utiliza Microsoft Flow para gestionar grupos de Office 365. Con este chatbot veremos
tecnologias como L.U.I.S. (Language Understanding Intelligent Service) y como crear el Bot Channel Registration para conectarlo con Teams

## Primeros pasos

Lo primero sera clonar el repositorio y configurar el fichero "appsettings.json" con nuestros recursos.
![](https://github.com/hectorgd/botwithflow/blob/master/BotWithFlow/images/appsettings.PNG)

Para conseguir el "MicrosoftAppId" y "MicrosoftAppPassword" vamos a crear un recurso en azure que se llama Bot Channel Registration

![](https://raw.githubusercontent.com/hectorgd/botwithflow/775e45442199b2353cee4e2434314da03400e0dd/BotWithFlow/images/channelcreation.PNG)

Una vez tengamos el recurso creado vamos a la sección de Configuracion y copiaremos el "MicrosoftAppId" y lo pegamos en el bot, y luego me daremos
a "Manage y generaremos una "MicrosoftAppPassword"
![](https://github.com/hectorgd/botwithflow/blob/master/BotWithFlow/images/appid.png)
![](https://github.com/hectorgd/botwithflow/blob/master/BotWithFlow/images/apppassword.PNG)

Ya tenemos el MicrosoftAppId y el AppPassword , ¿Y ahora que? Pues vamos a crear a LUIS para obtener sus settings 
En [https://github.com/hectorgd/botwithflow/blob/master/BotWithFlow/Luis/LuisWithFlow-v1.json](https://github.com/hectorgd/botwithflow/blob/master/BotWithFlow/Luis/LuisWithFlow-v1.json) os dejo el modelo de LUIS para que 
solo tengan que importarlo una vez tengamos creado nuestra aplicacion de LUIS

## Crear L.U.I.S.

Para crear nuestra app de LUIS vamos a [https://eu.luis.ai/home](https://eu.luis.ai/home) y crear aplicación.

Una vez la tengamos creada vamos a Administrar en el menu y en Versiones le damos a importar y seleccionaremos el json que compartí anteriormente
![](https://github.com/hectorgd/botwithflow/blob/master/BotWithFlow/images/importLUIS.png)

Una vez la tengamos importada le damos a Entrenar y a Publicar

Ahora vamos a sacar las "LuisAppId" y "LuisAPIKey".
Para la LuisAppId vamos a Información de la aplicacion y veremos el "ApplicationId"
![](https://github.com/hectorgd/botwithflow/blob/master/BotWithFlow/images/luisid.png)

Y para el LuisAppID y HostName vamos a la pestaña que esta justo debajo a la de "Recursos de Azure" y veremos la siguiente información 
![](https://github.com/hectorgd/botwithflow/blob/master/BotWithFlow/images/luisapppassword.png)
