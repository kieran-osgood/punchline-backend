module "managed-policies" {
  source = "yukihira1992/managed-policies/aws"
}

data "aws_iam_policy" "AdministratorAccess" {
  arn = module.managed-policies.AdministratorAccess
}

resource "aws_iam_user" "iamadmin" {
  name = "iamadmin"
}

resource "aws_iam_access_key" "iamadmin" {
  user = aws_iam_user.iamadmin.name
}

resource "aws_iam_user_ssh_key" "iamadmin" {
  username   = aws_iam_user.iamadmin.name
  encoding   = "SSH"
  public_key = "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQCZW+eiTwucS96jRmKvzjHRcmTJ/GoWQdyz3T4fT41qUlq0aH55ptWUgfVO2DPMsZiAMjsZoeI+E7JW/Z6/HZ6QapNuOHcyXJ/fj/AxDstbfL4gxVpuXmvmcKEjsUH/MfBMtcl16dhBKfJ3CA9SyxUzO5mMIHQHKK+z2J+fN8qPLCiC26C/MKvpZoxMbOKHTfb7BCNp5rOG6r/p74NudOABA2NrTb6atY9Vmyqf1hteJGWtE6yvWOeM78DVukExu/vChERQTsjxEk7dP62OASSer4iPOKVsNRoQ8ZHSP8RNeV5KXLZeFiUI5dLnDO9JhbDPmM/Ci7AhFzCWKhhgpYr6GXXOf+ebZuS3dSoplIhBo+kEV0MYpAJKbxGUCgyrfwt0kUKc3Kp69BiWtrGMo2++0bM+APjpAkN4yil5+voO/jXRONFV/F/o+2D0BB0JG1CgQ5e7QnXa2OOKnQCQdBr7bO8cHSADQXMtYQVtJpdJip6eb5HUiLbGJjeFfEajRSbL1hVqTLkKUvu8PVbL/U1ugKBe/qsnxg6hYDxIQnqIJeSvhKp16pB3gQ+tr/fB0ADKkkrc3w44zZnXGl61ZFwG5FhNAO0Tcv+/KILFPsbnTUSOH0bL9ix9HDpYUU5q6G5o2SxtVfZxBxmPtQuGh8q8PJWzAalHj3KUHOwE4lvE9w=="
  status     = "Active"

}

resource "aws_iam_policy" "iamadmin-ECRFullAccess" {
  name = "ECRFullAccess-Policy"

  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Sid" : "VisualEditor0",
        "Effect" : "Allow",
        "Action" : [
          "ecr:GetRegistryPolicy",
          "ecr:DescribeRegistry",
          "ecr:GetAuthorizationToken",
          "ecr:DeleteRegistryPolicy",
          "ecr:PutRegistryPolicy",
          "ecr:PutReplicationConfiguration"
        ],
        "Resource" : "*"
      },
      {
        "Sid" : "VisualEditor1",
        "Effect" : "Allow",
        "Action" : "ecr:*",
        "Resource" : aws_ecr_repository.punchline-backend.arn
      }
    ]
  })
}

resource "aws_iam_user_policy_attachment" "iamadmin-AdministratorAccess" {
  user = aws_iam_user.iamadmin.name
  policy_arn = data.aws_iam_policy.AdministratorAccess.arn
}

resource "aws_iam_user_policy_attachment" "iamadmin-ECRFullAccess" {
  user       = aws_iam_user.iamadmin.name
  policy_arn = aws_iam_policy.iamadmin-ECRFullAccess.arn
}
