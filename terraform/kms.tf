resource "aws_kms_key" "PunchlineEnvironmentSecretsKey" {
  description             = "This key is used with mozilla/sops for encrypting/decrypting the environment variables for the project"
  deletion_window_in_days = 10
}

#resource "aws_kms_key" "aws-acm" {
#  description             = "Default master key that protects my ACM private keys when no other key is defined"
#}