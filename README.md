
Install Azure CLI

https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest

Azure

We need to login to Azure
```
az login
```
To setup current subscription if you have more then one (especially other is client sub ;))
```
az account set -s SUBSCRIPTION_ID
```
Create new Resource Group
```
az group create --name WorkshopCube --location northeurope
```
Create  Container Registry:
```
az acr create --resource-group WorkshopCube --name patrykgaDockerContainerRegistry --sku Basic
```
Enable admin:
```
az acr update -n patrykgaDockerContainerRegistry --admin-enabled true
```
Get admin credentials:
```
az acr credential show --name patrykgaDockerContainerRegistry
```
Configure VSTS Package
