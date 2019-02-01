
Install Azure CLI or you can use Azure Power Shell

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

Because it take some time, now we need to create Azure Kubernetes Service (AKS):
```
az aks create --resource-group WorkshopCube --name WorkshopAKS --node-count 3 --enable-addons monitoring --generate-ssh-keys
```
I assume you have ready .net core docker project repository. Then we need to:

Configure VSTS Package

We need to open VSTS (Azure DevOps project)

---Commit GIT Project

Back to Azure
if you run az cli commands from Azure Powershell you should store your ssh keys

