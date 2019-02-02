## Introduction to release .net core app to Azure Kubernetes Service (AKS)

#### What we will do?:
- We have .net core app
- We will cretae Azure Container Registry Service (ACR)
- We will configure Azure DevOps (old VSTS) connection to ACR Service
- We will configure Azure DevOps (old VSTS) build (build Image and Push to ACR Service)
- We will create Azure Kubernetes Service (AKS)
- We will create Azure DevOps (VSTS) Release

We will have AKS with Load Balancer.

####Let's get started!

Install Azure CLI or you can use Azure Power Shell

https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest

#### Azure

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

#### Container Registry Service

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

(You will need this credentials to configure Azure DevOps (VSTS))

#### Create Azure Kubernetes Service

Because it take some time, now we will create Azure Kubernetes Service (AKS):
```
az aks create --resource-group WorkshopCube --name WorkshopAKS --node-count 3 --enable-addons monitoring --generate-ssh-keys
```

#### Configure Azure DevOps (VSTS) Package Pipelines

Configure VSTS Package

We need to open VSTS (Azure DevOps project)

---Commit GIT Project

Back to Azure
if you run az cli commands from Azure Powershell you should store your ssh keys

