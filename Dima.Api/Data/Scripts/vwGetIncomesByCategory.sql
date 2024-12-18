DROP VIEW IF EXISTS "vwGetIncomesByCategory";

CREATE VIEW "vwGetIncomesByCategory" AS
SELECT
    "Transaction"."UserId",
    "Category"."Title" AS "Category",
    EXTRACT(YEAR FROM "Transaction"."PaidOrReceivedAt") AS "Year",
    SUM("Transaction"."Amount") AS "Incomes"
FROM
    "Transaction"
        INNER JOIN "Category"
                   ON "Transaction"."CategoryId" = "Category"."Id"
WHERE
    "Transaction"."PaidOrReceivedAt"
        >= CURRENT_DATE - INTERVAL '11 months'
  AND "Transaction"."PaidOrReceivedAt"
    < CURRENT_DATE + INTERVAL '1 month'
  AND "Transaction"."Type" = 1
GROUP BY
    "Transaction"."UserId",
    "Category"."Title",
    EXTRACT(YEAR FROM "Transaction"."PaidOrReceivedAt");