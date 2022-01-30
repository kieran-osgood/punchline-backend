resource "aws_security_group" "alb-ecs-sg" {
  name        = "alb-ecs-sg"
  description = "Allow http/https access for the application load balancer on the ecs cluster"
  vpc_id      = "vpc-07b2efe3a69664c10"
}
