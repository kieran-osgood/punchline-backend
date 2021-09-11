#!/bin/sh

export file="$HOME/.aws/credentials"

touch "$file"
{
	echo
	echo "[$AWS_CREDENTIAL_PROFILE]" 
	echo "aws_access_key_id = [$AWS_ACCESS_KEY_ID]"
	echo "aws_secret_access_key = [$AWS_SECRET_ACCESS_KEY]" 
} >> "$file"
