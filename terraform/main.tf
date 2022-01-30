// Provider configuration
terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 3.0"
    }
  }
}

provider "aws" {
  region = "us-east-1"
}

resource "aws_ecs_cluster" "punchline-backend-cluster" {
  name = "punchline-backend-cluster"
  setting {
    name  = "containerInsights"
    value = "disabled"
  }
}
