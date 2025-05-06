#!/usr/bin/env zsh

# install pre-requisites if not already installed on your machine / laptop
brew install --cask docker 
brew tap fluxcd/tap
brew install colima \
    qemu \
    kubectl \
    k9s \
    k3d \
    helm \
    docker-buildx \
    docker-compose \
    fluxcd/tap/flux \
    kubeseal

# start colima VM instance using Qemu (quick emulator)
colima start \
    --arch aarch64 \
    --vm-type qemu \
    --memory 16 \
    --cpu 4 \
    --dns 1.1.1.1 \
    --dns 1.0.0.1 \
    --network-address

# ssh into the VM and update its packages
colima ssh
# inside the VM 
# run sudo apt update && sudo apt full-upgrade -y  && sudo apt install -y curl iputils-ping dnsutils net-tools && exit

# restart colima to ensure VM is restarted
colima restart

colima list #-- need to write down the IP address
## [put the IP here] --> 192.168.106.2

# we'll use the dns service nip.io to have free dns record in the below format
# k3d.192.168.106.2.nip.io
# we'll use this dns to update k3d config file

# use openssl to generate secretToken
openssl rand -base64 12


#deploy k3d
k3d cluster create -c ./k3d.yaml # we disabled traefik to have nginx-ingress instead.
# check if ready
kubectl get nodes # notice the status of the nodes showing "NotReady"

#taint server so it doesn't get any workload scheduled
kubectl taint nodes k3d-bootcamp-server-0 node-role.kubernetes.io/infra=:NoSchedule

# update dnscore settings to use 1.1.1.1 instead of default --> coredns -> docker -> your laptop -> your router -> your ISP.
kubectl edit cm -n kube-system coredns
#
# ### forward . /etc/resolv.conf
# forward . tls://1.1.1.1 tls://1.0.0.1 {
#   tls_servername cloudflare-dns.com
#   health_check 5s
#}

#install calico & custom resources
kubectl create -f https://raw.githubusercontent.com/projectcalico/calico/v3.29.3/manifests/tigera-operator.yaml
kubectl apply -f custom-resources.yaml
kubectl patch installation default --type=merge --patch='{"spec":{"calicoNetwork":{"containerIPForwarding":"Enabled"}}}'

# wait for couple of minutes.
# sometimes it is good to restart colima

# check kubernetes status
kubectl get nodes # --> should indicate ready.

# install nginx ingress
helm upgrade --install ingress-nginx ingress-nginx \
  --repo https://kubernetes.github.io/ingress-nginx \
  --namespace ingress-nginx --create-namespace

# install kubeseal helm chart
helm repo add sealed-secrets https://bitnami-labs.github.io/sealed-secrets
helm upgrade --install  sealed-secrets -n kube-system --set-string fullnameOverride=sealed-secrets-controller sealed-secrets/sealed-secrets


# install cert-manager using helm
helm repo add jetstack https://charts.jetstack.io --force-update

helm upgrade --install  \
  cert-manager jetstack/cert-manager \
  --namespace cert-manager \
  --create-namespace \
  --version v1.17.2 \
  --set crds.enabled=true


# check on cert-manager site, how to setup acmeDNS issuer for cloudflare  
# have the token from your dens provider.
# use kubeseal to seal the secret and have a commitable file (no risk to have it inside git repo)



cat cloudflare-token.yaml | kubeseal \
      --controller-name=sealed-secrets-controller \
      --controller-namespace=kube-system \
      --format yaml > sealed-cloudflare-token.yaml

kubectl apply -f sealed-cloudflare-token.yaml
kubectl apply -f cloudflare-issuer.yaml

# verify via k9s
######## end session 1 ##############

# install gitea server
helm repo add gitea-charts https://dl.gitea.com/charts/
helm upgrade --install gitea gitea-charts/gitea \
  --namespace devsecops --create-namespace \
  --set gitea.admin.username=gitea_admin \
  --set gitea.admin.password=wKuEf1krIb+1Gff6jocnpg== \
  --set gitea.config.server.ROOT_URL=https://git.bootcamp.althunibat.xyz/

kubectl apply -f gitea.ingress.yaml

# containers can't internally interact with VM IP (used by dns / cloudflare) 
# so we need to update drone container by overriding the dns of gitea to point to
# nginx service IP (internal cluster IP). --> use K9s to do so.

# wait until the tls is ready.
# login into the gitea using the url https://git.bootcamp.althunibat.xyz
# create application from site admin
# name: DRONE CI
# callback url: https://drone-ci.bootcamp.althunibat.xyz/login
# edit and regenrate client secret ==> copy that
# gto_ugoqmuigtmzivapl6zzks65ot6sqyqdysdnvvvldav5lcv7hmrlq

# create shared secret
# openssl rand -hex 16
# 547a6cda431d60a909b1f0f91f2deccd
# update drone-ci.yaml
# install drone
kubectl apply -f drone-ci.yaml

# deploy drone runner using Colima VM and Docker directly

docker pull drone/drone-runner-docker:1

docker run --detach \
  --volume=/var/run/docker.sock:/var/run/docker.sock \
  --env=DRONE_RPC_PROTO=https \
  --env=DRONE_RPC_HOST=drone-ci.bootcamp.althunibat.xyz \
  --env=DRONE_RPC_SECRET=547a6cda431d60a909b1f0f91f2deccd  \
  --env=DRONE_RUNNER_CAPACITY=2 \
  --env=DRONE_RUNNER_NAME=docker-runner \
  --publish=3000:3000 \
  --restart=always \
  --name=runner \
  drone/drone-runner-docker:1

  # validate connectivity through docker logs 

  ######## end session 2 ##############

  # create user in gitea
  # login into gitea using docker ==> create and use personal token
  # build the image locally, push it into repository.
  # install kustomize cli
  brew install kustomize
 # explain the deployment procedure using kustomize
 
 # change directlry to deploy
 kubectl create ns myapp

 # generate password for postgresql and update the secret file
 openssl rand -base64 16
 # use kubeseal to generate sealed secret
 cat pgsql-secret.yaml | kubeseal \
      --controller-name=sealed-secrets-controller \
      --controller-namespace=kube-system \
      --format yaml > sealed-pgsql-secret.yaml
 # delete the original secret file
 # build kustomization file
 kustomize init --autodetect
 kustomize edit set namespace myapp
 kustomize edit set image webapi=git.bootcamp.althunibat.xyz/gitea_admin/webapi:0.0.1-dev-1
 kubectl apply -k .
