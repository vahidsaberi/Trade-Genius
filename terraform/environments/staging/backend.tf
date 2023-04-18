terraform {
  backend "s3" {
    bucket         = "trade-genius-backend"
    key            = "api/staging/terraform.tfstate"
    region         = "ap-south-1"
    dynamodb_table = "trade-genius-state-locks"
  }
}
