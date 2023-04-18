resource "aws_s3_bucket" "s3_bucket" {
  bucket = "trade-genius-backend"
  tags = {
    Name = "trade-genius-backend"
  }
  lifecycle {
    prevent_destroy = true
  }
}
