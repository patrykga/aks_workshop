## Introduction to release .net core app to Azure Kubernetes Service (AKS)

#### What we will do?:
- We have .net core app
- We will cretae Azure Container Registry Service (ACR)
- We will configure Azure DevOps (old VSTS) connection to ACR Service
- We will configure Azure DevOps (old VSTS) build (build Image and Push to ACR Service)
- We will create Azure Kubernetes Service (AKS)
- We will create Azure DevOps (VSTS) Release

We will have AKS with Load Balancer.

#### Let's get started!

Install Azure CLI

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

(You will need this credentials to configure Azure DevOps (VSTS))

#### Create Azure Kubernetes Service

Because it take some time, now we will create Azure Kubernetes Service (AKS):
```
az aks create --resource-group WorkshopCube --name WorkshopAKS --node-count 3 --enable-addons monitoring --generate-ssh-keys
```

#### Configure Azure DevOps (VSTS) Package Pipelines

Configure VSTS Package

We need to open VSTS (Azure DevOps project)

Create Build Package using YAML
Configuration:
build:
Project Name: $(Build.Repository.Name)
Action: Build Service Images
Additional Image Tags: $(Build.BuildNumber)
push:
Action: Push Service Images

I will describe release process

We back to kubernetes:

We need to get Container Registry Credentials:

Get admin credentials:
```
az acr credential show --name patrykgaDockerContainerRegistry
```

We need to install kubectl

```
az aks install-cli
```

Set kubectl path in Powershell, sample:

```
$env:path += 'C:\Users\patrykga\.azure-kubectl'
```

Get credentials for your cubernetes:

```
az aks get-credentials --resource-group WorkshopCube --name WorkshopAKS
```

Create Kubernetes secret (connect to registry to pull images):

```
kubectl create secret docker-registry acr-auth --docker-server <acr-login-server> --docker-username <service-principal-ID> --docker-password <service-principal-password>
```
We need to install chocolate to install helm (Windows Package Manager)

```
Set-ExecutionPolicy Bypass -Scope Process -Force; `iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
```

We need to install helm

```
choco install kubernetes-helm
```

We need to install Helm

```
helm init
```

Verify helm installation:

```
kubectl describe deploy tiller-deploy --namespace=kube-system
```

We need to install nginx ingress:
```
helm install stable/nginx-ingress
```

We will get error, we need to create specjal account:
```
kubectl create serviceaccount --namespace kube-system tiller
kubectl create clusterrolebinding tiller-cluster-rule --clusterrole=cluster-admin --serviceaccount=kube-system:tiller
helm init --service-account tiller --upgrade
helm repo update
```

Now we can again try to install nginx-ingress:
```
helm install stable/nginx-ingress
```
We have Kubernetes and Helm Package Manager.

Open Kubernetes Dashboard:

```
az aks browse --resource-group WorkshopCube --name WorkshopAKS
```
Error

Create ClusterRoleBinding
```
kubectl create clusterrolebinding kubernetes-dashboard -n kube-system --clusterrole=cluster-admin --serviceaccount=kube-system:kubernetes-dashboard
```

We can open Kubernetes Dashboard now:
```
az aks browse --resource-group WorkshopCube --name WorkshopAKS
```
