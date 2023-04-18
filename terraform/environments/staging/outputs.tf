output "api_url" {
  value       = "http://${aws_lb.trade_genius_api_alb.dns_name}/api"
  description = "Trade-Genius .NET WebAPI URL"
}
