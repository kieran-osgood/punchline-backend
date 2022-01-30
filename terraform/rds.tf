resource "aws_db_instance" "punchline-backend-mysql" {
  allocated_storage    = 20
  engine               = "mysql"
  engine_version       = "8.0.23"
  instance_class       = "db.t2.micro"
  username             = "admin"
  parameter_group_name = "default.mysql8.0"
  skip_final_snapshot  = true
  deletion_protection  = false
  publicly_accessible = true
  max_allocated_storage = 1000
  copy_tags_to_snapshot = true
}
