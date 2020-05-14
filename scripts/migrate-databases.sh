#!/usr/bin/env bash
set -e

echo "Database Migration..."

app_guid=`cf app $1 --guid`
#credentials=`cf curl /v2/apps/$app_guid/env | jq '.system_env_json.VCAP_SERVICES | if .["p-mysql"] != null then .["p-mysql"] elif .["p.mysql"] != null then .["p.mysql"] else .["cleardb"] end | .[0].credentials'`
credentials=`cf curl /v2/service_keys | jq '.resources[] | select(.entity.name=="tracker-user-key") | .entity.credentials'`

ip_address=`echo $credentials | jq -r '.hostname'`
db_name=`echo $credentials | jq -r '.name'`
db_username=`echo $credentials | jq -r '.username'`
db_password=`echo $credentials | jq -r '.password'`

echo "app_guid=$app_guid"
echo "credentials=$credentials"
echo "ip_address=$ip_address"
echo "db_name=$db_name"
echo "db_username=$db_username"
echo "db_password=$db_password"

echo "Opening ssh tunnel to $ip_address"
cf ssh -N -L 63306:$ip_address:3306 $1 &
cf_ssh_pid=$!

echo "Waiting for tunnel"
sleep 5

# Passing this in as a param is a bit strage. Maybe put flyway on the path?
./flyway-*/flyway -url="jdbc:mysql://127.0.0.1:63306/$db_name" -locations=filesystem:$2/databases/tracker -user=$db_username -password=$db_password migrate

kill -STOP $cf_ssh_pid
