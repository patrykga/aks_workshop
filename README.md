## Introduction to release .net core app to Azure Kubernetes Service (AKS)

#### What we will do?:
- We have .net core app
- We will create Azure Container Registry Service (ACR)
- We will configure Azure DevOps (old VSTS) connection to ACR Service
- We will configure Azure DevOps (old VSTS) build (build Image and Push to ACR Service)
- We will create Azure Kubernetes Service (AKS)
- We will create Azure DevOps (VSTS) Release ??

We will have AKS with Load Balancer.
In this tutorial we will use Powershell

#### Let's get started!

Install Azure CLI

https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest

#### Azure

We need to login to Azure:
```
az login
```
To setup current subscription if you have more then one:
```
az account set -s SUBSCRIPTION_ID
```
We need to add resource group name to variable (you need to change this to your own name)
```
$resourceGroupName = "WorkshopCube"
```
Create new Resource Group:
```
az group create --name $resourceGroupName --location northeurope
```

#### Container Registry Service

Set variable for container name (you need to change this to your own name):
```
$dockerRegistryContainerName = "patrykgaDockerContainerRegistry"
```

Create  Container Registry:
```
az acr create --resource-group $resourceGroupName --name $dockerRegistryContainerName --sku Basic
```
Enable admin:
```
az acr update -n $dockerRegistryContainerName --admin-enabled true
```

(You will need this credentials to configure Azure DevOps (VSTS) and kubernetes secret to pull images)

#### Create Azure Kubernetes Service

We need to add AKS name to variable:
```
$aksName = "WorkshopAKS"
```

Because it take some time, now we will create Azure Kubernetes Service (AKS):
```
az aks create --resource-group $resourceGroupName --name $aksName --node-count 3 --enable-addons monitoring --generate-ssh-keys
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

[Describe Release Process]

We back to kubernetes:

We need to get Container Registry Credentials:

Get admin credentials:
```
az acr credential show --name $dockerRegistryContainerName
```

We need to install kubectl:
```
az aks install-cli
```

Set kubectl path in Powershell, sample (you need to update it to your own path):
```
$env:path += 'C:\Users\patrykga\.azure-kubectl'
```

Get credentials for your kubernetes:
```
az aks get-credentials --resource-group $resourceGroupName --name $aksName
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

We need to init Helm
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

We will get error 
We need to create specjal account:
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
az aks browse --resource-group $resourceGroupName --name $aksName
```

We will get error

Create ClusterRoleBinding
```
kubectl create clusterrolebinding kubernetes-dashboard -n kube-system --clusterrole=cluster-admin --serviceaccount=kube-system:kubernetes-dashboard
```

We can open Kubernetes Dashboard now:
```
az aks browse --resource-group $resourceGroupName --name $aksName
```

We need to go to deployment\chart-api directory and run
```
helm install --namespace default --name kuberapi ./
```

Next we need to go to deployment\char-mvc directory and run
```
helm install --namespace default --name kubermvc ./
```
