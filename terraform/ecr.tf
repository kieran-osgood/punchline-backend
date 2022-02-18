resource "aws_ecr_repository" "punchline-backend" {
  name                 = "punchline-backend"
  image_tag_mutability = "IMMUTABLE"

  encryption_configuration {
	encryption_type = "AES256"
  }

  image_scanning_configuration {
    scan_on_push = true
  }
}