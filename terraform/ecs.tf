resource "aws_ecs_cluster" "backend" {
  name = "punchline-backend-cluster"

  setting {
    name  = "containerInsights"
    value = "disabled"
  }
}

# resource "aws_ecs_service" "mongo" {
#   name            = "mongodb"
#   cluster         = aws_ecs_cluster.foo.id
#   task_definition = aws_ecs_task_definition.mongo.arn
#   desired_count   = 3
#   iam_role        = aws_iam_role.foo.arn
#   depends_on      = [aws_iam_role_policy.foo]

#   ordered_placement_strategy {
#     type  = "binpack"
#     field = "cpu"
#   }

#   load_balancer {
#     target_group_arn = aws_lb_target_group.foo.arn
#     container_name   = "mongo"
#     container_port   = 8080
#   }

#   placement_constraints {
#     type       = "memberOf"
#     expression = "attribute:ecs.availability-zone in [us-west-2a, us-west-2b]"
#   }
# }

resource "aws_ecs_task_definition" "punchline-backend" {
  family = "container-of-punchline-backend"
  cpu    = "256"
  memory = "1024"
  requires_compatibilities = [
    "FARGATE",
  ]
  task_role_arn      = "arn:aws:iam::345637428723:role/ecsTaskExecutionRole"
  execution_role_arn = "arn:aws:iam::345637428723:role/ecsTaskExecutionRole"
  tags_all           = {}
  tags               = {}
  skip_destroy       = false
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
