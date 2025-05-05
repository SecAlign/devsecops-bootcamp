#!/usr/bin/env zsh

# install pre-requisites if not already installed
brew reinstall colima \
    quemu \
    kubectl \
    k9s \
    k3d \
    helm \
    docker \
    docker-buildx \
    docker-compose \
    flux \
    kubeseal

# start colima VM instance using Qemu (quick emulator)
colima start \
    --arch aarch64 \
    --vm-type qemu \
    --memory 16 \
    --cpu 4 \
    --network-address

# ssh into the VM and update its packages
colima ssh

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
k3d cluster create -c ./k3d.yaml
# check if ready
kubectl get nodes # notice the status of the nodes showing "NotReady"

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

# check kubernetes status
kubectl get nodes # --> should indicate ready.


# install kubeseal helm chart
helm repo add sealed-secrets https://bitnami-labs.github.io/sealed-secrets
helm install sealed-secrets -n kube-system --set-string fullnameOverride=sealed-secrets-controller sealed-secrets/sealed-secrets


# install cert-manager using helm
helm repo add jetstack https://charts.jetstack.io --force-update

helm install \
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



