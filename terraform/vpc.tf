resource "aws_vpc" "Punchline" {
  cidr_block = "10.0.0.0/16"
  tags = {
    "Name" = "Punchline"
  }
  tags_all = {
    "Name" = "Punchline"
  }
}
