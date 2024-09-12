$ResourceGroup = 'gap-azure-lab'
$Location = 'eastus2'
$Namespace = 'gap-lab-ns'
$EventHub = 'gap-lab-hub'
$ConsumerGroup = 'gap-lab-cg'
$Identity = 'gap-lab-identity'

# Create RG:
az group create --name $ResourceGroup --location $Location
# Create MI:
az identity create --name $Identity -g $ResourceGroup --location $Location
# Get MI id:
$ManagementId = az identity show --name $Identity -g $ResourceGroup --query id -o json | ConvertFrom-json
# Create namespace:
$NameSpaceName = az eventhubs namespace create -g $ResourceGroup --name $Namespace --location $Location --sku Standard --enable-auto-inflate --maximum-throughput-units 1 --mi-user-assigned $ManagementId --query name -o json | convertFrom-Json
# Setup eventHub:
az eventhubs eventhub create -g $ResourceGroup --namespace-name $Namespace --name $EventHub --cleanup-policy Delete --partition-count 1
# Get Id Event Hub:
$EventHubId = az eventhubs namespace show --name $Namespace -g $ResourceGroup --query id -o json | ConvertFrom-Json
# Create Consumer Group:
az eventhubs eventhub consumer-group create --consumer-group-name $ConsumerGroup --eventhub-name $EventHub --namespace-name $Namespace -g $ResourceGroup
# Get Principal Id from MI:
$MIPrincipalId = az identity show --name $Identity -g $ResourceGroup --query principalId -o json | ConvertFrom-Json
# Set Role to Event Hub:
$RoleAssignment = az role assignment create --assignee-object-id $MIPrincipalId --assignee-principal-type ServicePrincipal --role 'Azure Event Hubs Data Owner'--scope $EventHubId --query createdOn -o json | ConvertFrom-Json