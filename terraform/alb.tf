resource "aws_lb" "alb-ecs-api-cluster" {
  name               = "alb-ecs-api-cluster"
  internal           = false
  load_balancer_type = "application"
  security_groups    = ["sg-0d88c06133406175b"]
#   security_groups    = [aws_security_group.alb-ecs-sg.id]
#   subnets            = [for subnet in aws_subnet.public : subnet.id]
  subnets            = [
	  aws_subnet.punchline-web-A.id,
	  aws_subnet.punchline-web-B.id,
  ]

  enable_deletion_protection = false

#   access_logs {
#     bucket  = aws_s3_bucket.lb_logs.bucket
#     prefix  = "test-lb"
#     enabled = true
#   }

#   tags = {
#     Environment = "production"
#   }
}
