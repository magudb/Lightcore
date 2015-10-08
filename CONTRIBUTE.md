#Getting the Lightcore solution up and running#

1. Fork / Clone
2. Unzip the **Website** folder from "Sitecore 8.0 rev. 150812.zip" into `.\src\Lightcore.Server.Sitecore.TestServer`
3. Unzip the **Website\bin** folder from "Sitecore 8.0 rev. 150812.zip" into `.\lib\Sitecore`
4. Undo any commits (the zip file contains Web.config and ConnectionStrings.config)
5. Build
6. Setup IIS site with a binding on `lightcore-testserver.ad.codehouse.com`
7. Browse `lightcore-testserver.ad.codehouse.com` and log into Sitecore
8. Click revert database in the serialization tab
9. Do a full publish