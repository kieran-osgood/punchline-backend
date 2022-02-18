resource "aws_lb" "alb-ecs-api-cluster" {
  name               = "alb-ecs-api-cluster"
  internal           = false
  load_balancer_type = "application"
  security_groups    = ["sg-0d88c06133406175b"]
  #   security_groups    = [aws_security_group.alb-ecs-sg.id]
  #   subnets            = [for subnet in aws_subnet.public : subnet.id]
  subnets = [
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

resource "aws_lb_target_group" "tg-http-api-cluster" {
  name                               = "tg-http-api-cluster"
  port                               = 80
  protocol                           = "HTTP"
  vpc_id                             = aws_vpc.Punchline.id
  connection_termination             = false
  lambda_multi_value_headers_enabled = false
  load_balancing_algorithm_type      = "round_robin"
  protocol_version                   = "HTTP1"
  proxy_protocol_v2                  = false
  tags                               = {}
  tags_all                           = {}
  target_type                        = "ip"

  health_check {
    enabled             = true
    healthy_threshold   = 5
    interval            = 30
    matcher             = "200"
    path                = "/healthcheck"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = 5
    unhealthy_threshold = 2
  }

  stickiness {
    type            = "lb_cookie"
    cookie_duration = 86400
    enabled         = false
  }
}
