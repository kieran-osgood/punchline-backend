resource "aws_vpc" "Punchline" {
  cidr_block = "10.0.0.0/16"
  tags = {
    "Name" = "Punchline"
  }
  tags_all = {
    "Name" = "Punchline"
  }
}

resource "aws_subnet" "punchline-web-A" {
  vpc_id                  = aws_vpc.Punchline.id
  map_public_ip_on_launch = true
  cidr_block              = "10.0.0.0/19"

  tags = {
    Name = "punchline-web-A"
  }
}

resource "aws_subnet" "punchline-web-B" {
  vpc_id                  = aws_vpc.Punchline.id
  map_public_ip_on_launch = true
  cidr_block              = "10.0.128.0/19"

  tags = {
    Name = "punchline-web-B"
  }
}

resource "aws_subnet" "punchline-database-A" {
  vpc_id                  = aws_vpc.Punchline.id
  map_public_ip_on_launch = true
  cidr_block              = "10.0.32.0/19"

  tags = {
    Name = "punchline-database-A"
  }
}

resource "aws_subnet" "punchline-database-B" {
  vpc_id                  = aws_vpc.Punchline.id
  map_public_ip_on_launch = false
  cidr_block              = "10.0.160.0/19"

  tags = {
    Name = "punchline-database-B"
  }
}


resource "aws_subnet" "punchline-private-A" {
  vpc_id                  = aws_vpc.Punchline.id
  map_public_ip_on_launch = false
  cidr_block              = "10.0.64.0/19"

  tags = {
    Name = "punchline-private-A"
  }
}

resource "aws_subnet" "punchline-private-B" {
  vpc_id                  = aws_vpc.Punchline.id
  map_public_ip_on_launch = false
  cidr_block              = "10.0.192.0/19"

  tags = {
    Name = "punchline-private-B"
  }
}


resource "aws_subnet" "punchline-reserved-A" {
  vpc_id                  = aws_vpc.Punchline.id
  map_public_ip_on_launch = false
  cidr_block              = "10.0.96.0/19"

  tags = {
    Name = "punchline-reserved-A"
  }
}

resource "aws_subnet" "punchline-reserved-B" {
  vpc_id                  = aws_vpc.Punchline.id
  map_public_ip_on_launch = false
  cidr_block              = "10.0.224.0/19"

  tags = {
    Name = "punchline-reserved-B"
  }
}
