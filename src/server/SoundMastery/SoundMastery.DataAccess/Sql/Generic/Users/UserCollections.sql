SELECT role."Id", role."Name"
FROM "SoundMastery"."IdentityUserRole" userRole
INNER JOIN "SoundMastery"."Roles" role ON userRole."RoleId" = role."Id"
WHERE userRole."UserId" = @Id;

SELECT * FROM "SoundMastery"."RefreshTokens"
WHERE "RefreshTokens"."UserId" = @Id;
