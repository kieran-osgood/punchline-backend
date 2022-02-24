resource "aws_ecs_cluster" "backend" {
  name = "punchline-backend-cluster"
  setting {
    name  = "containerInsights"
    value = "disabled"
  }
}

resource "aws_ecs_service" "punchline-backend" {
  name                              = "punchline-backend-service"
  desired_count                     = 1
  enable_ecs_managed_tags           = true
  health_check_grace_period_seconds = 60
  iam_role                          = "aws-service-role"
  depends_on                        = [module.managed-policies.AmazonECSServiceRolePolicy]
  launch_type                       = "EC2"
  platform_version                  = "LATEST"
  cluster                           = aws_ecs_cluster.backend.id
  task_definition                   = aws_ecs_task_definition.punchline-backend.arn
  wait_for_steady_state             = false
  tags                              = {}
  tags_all                          = {}
  deployment_circuit_breaker {
    enable   = false
    rollback = false
  }

  timeouts {}

  network_configuration {
    assign_public_ip = true
    security_groups = [
      "sg-04c26a14ebff72546",
    ]
    subnets = [
      aws_subnet.punchline-web-A.id,
      aws_subnet.punchline-web-B.id
    ]
  }

  load_balancer { # forces replacement
    container_name   = "container-of-punchline-backend"
    container_port   = 80
    target_group_arn = aws_lb_target_group.tg-http-api-cluster.arn
  }
}

resource "aws_ecs_task_definition" "punchline-backend" {
  family = "container-of-punchline-backend"
  cpu    = "256"
  memory = "1024"
  requires_compatibilities = [
    "EC2",
  ]
  # TODO: replace with the managed policies module
  task_role_arn = "arn:aws:iam::345637428723:role/ecsTaskExecutionRole"
  # TODO: replace with the managed policies module
  execution_role_arn = "arn:aws:iam::345637428723:role/ecsTaskExecutionRole"
  tags_all           = {}
  tags               = {}
  skip_destroy       = false
  # TODO: replace this to be the set with image of latest
  container_definitions = jsonencode([
    {
      name              = "container-of-punchline-backend"
      image             = "345637428723.dkr.ecr.us-east-1.amazonaws.com/punchline-backend:6b3a4ca4f4347f756780896aa1f744a1af165b3c"
      cpu               = 0
      environment       = []
      mountPoints       = []
      essential         = true
      memoryReservation = 1024
      portMappings = [
        {
          containerPort = 80
          hostPort      = 80
          protocol      = "tcp"
        }
      ]
      volumesFrom = []
    }
  ])
}

resource "aws_ecs_cluster_capacity_providers" "ec2_provider" {
  cluster_name       = aws_ecs_cluster.backend.name
  capacity_providers = [aws_ecs_capacity_provider.ec2_provider.name]

  default_capacity_provider_strategy {
    base              = 1
    weight            = 100
    capacity_provider = aws_ecs_capacity_provider.ec2_provider.name
  }
}

resource "aws_ecs_capacity_provider" "ec2_provider" {
  name                           = "ec2_provider"
  managed_termination_protection = "ENABLED"

  managed_scaling {
    maximum_scaling_step_size = 1
    minimum_scaling_step_size = 1
    status                    = "ENABLED"
    target_capacity           = 1
  }

  auto_scaling_group_provider {
    auto_scaling_group_arn = aws_autoscaling_group.backend.arn
  }
}

resource "aws_autoscaling_group" "backend" {
  availability_zones = ["us-east-1a"]
  desired_capacity   = 1
  max_size           = 1
  min_size           = 1

  tag {
    key                 = "AmazonECSManaged"
    value               = true
    propagate_at_launch = true
  }

  launch_template {
    id      = aws_launch_template.punchline-backend.id
    version = "$Latest"
  }
}

resource "aws_launch_template" "punchline-backend" {
  name_prefix   = "punchline-backend"
  image_id      = "ami-1a2b3c"
  instance_type = "t2.micro"
}
