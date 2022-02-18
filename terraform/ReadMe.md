# How to update state for things that change regularly
Things like "last_restorable_date" on RDS and "etag" in s3 will change frequently, spamming the console on a plan.
Running `terraform apply -refresh-only` will refresh the state without affecting any infrastructure, helping reduce the noise.