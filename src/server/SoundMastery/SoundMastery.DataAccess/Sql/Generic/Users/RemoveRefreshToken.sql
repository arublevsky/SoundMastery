DELETE FROM "SoundMastery"."RefreshTokens"
WHERE "RefreshTokens"."UserId" = @UserId and  "RefreshTokens"."Token" = @Token
