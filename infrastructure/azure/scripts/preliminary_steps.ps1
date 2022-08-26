az account list-locations -o table

az group create -l westeurope -n macel94.github.io

az ad sp create-for-rbac --name macel94.github.io-sp-provider --role owner --scopes /subscriptions/<SUBSCRIPTION-ID>/resourceGroups/macel94.github.io

#Copy the full output from the previous step inside the GitHub secret key AZURE_CREDENTIALS. 
#You can set AZURE_CREDENTIALS in GitHub Setting-->Secret-->Actions
#You can set AZURE_SUBSCRIPTION in GitHub Setting-->Secret-->Actions