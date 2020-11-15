UPDATE "SoundMastery"."Users"
SET
    "EmailConfirmed"=@EmailConfirmed,
    "SecurityStamp"=@SecurityStamp,
    "PhoneNumber"=@PhoneNumber,
    "PhoneNumberConfirmed"=@PhoneNumberConfirmed,
    "TwoFactorEnabled"=@TwoFactorEnabled,
    "LockoutEnd"=@LockoutEnd,
    "LockoutEnabled"=@LockoutEnabled,
    "AccessFailedCount"=@AccessFailedCount,
    "FirstName"=@FirstName,
    "LastName"=@LastName
WHERE "Id" = @Id;
