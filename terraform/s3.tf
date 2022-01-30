resource "aws_s3_bucket" "punch-line" {
  bucket                      = "punch-line"
  acl                         = "private"
  force_destroy               = false

  website {
    index_document = "index.html"
  }
}

resource "aws_s3_bucket" "alb-access-logs547" {
  bucket        = "alb-access-logs547"
  acl           = "private"
  force_destroy = false
}
