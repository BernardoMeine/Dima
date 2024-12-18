DROP VIEW IF EXISTS "vwGetIncomesAndExpenses";

CREATE VIEW "vwGetIncomesAndExpenses" AS
SELECT
    "Transaction"."UserId",
    EXTRACT(MONTH FROM "Transaction"."PaidOrReceivedAt") AS "Month",
    EXTRACT(YEAR FROM "Transaction"."PaidOrReceivedAt") AS "Year",
    SUM(CASE WHEN "Transaction"."Type" = 1 THEN "Transaction"."Amount" ELSE 0 END) AS "Incomes",
    SUM(CASE WHEN "Transaction"."Type" = 2 THEN "Transaction"."Amount" ELSE 0 END) AS "Expenses"
FROM
    "Transaction"
WHERE
    "Transaction"."PaidOrReceivedAt" >= CURRENT_DATE - INTERVAL '11 months'
        AND "Transaction"."PaidOrReceivedAt" < CURRENT_DATE + INTERVAL '1 month'
        GROUP BY
        "Transaction"."UserId",
        EXTRACT(MONTH FROM "Transaction"."PaidOrReceivedAt"),
        EXTRACT(YEAR FROM "Transaction"."PaidOrReceivedAt");

