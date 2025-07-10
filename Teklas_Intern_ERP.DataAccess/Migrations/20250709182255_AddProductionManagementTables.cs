using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductionManagementTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MaterialMovements",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MaterialCategories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "BillOfMaterials",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BOMCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BOMName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductMaterialCardId = table.Column<long>(type: "bigint", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BOMType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RouteCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StandardTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SetupTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillOfMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillOfMaterials_MaterialCards_ProductMaterialCardId",
                        column: x => x.ProductMaterialCardId,
                        principalTable: "MaterialCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillOfMaterialItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillOfMaterialId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialCardId = table.Column<long>(type: "bigint", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ScrapFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ComponentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false),
                    IsPhantom = table.Column<bool>(type: "bit", nullable: false),
                    IssueMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OperationSequence = table.Column<int>(type: "int", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CostAllocation = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SupplierMaterialCardId = table.Column<long>(type: "bigint", nullable: true),
                    LeadTimeOffset = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillOfMaterialItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillOfMaterialItems_BillOfMaterials_BillOfMaterialId",
                        column: x => x.BillOfMaterialId,
                        principalTable: "BillOfMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillOfMaterialItems_MaterialCards_MaterialCardId",
                        column: x => x.MaterialCardId,
                        principalTable: "MaterialCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillOfMaterialItems_MaterialCards_SupplierMaterialCardId",
                        column: x => x.SupplierMaterialCardId,
                        principalTable: "MaterialCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkOrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BillOfMaterialId = table.Column<long>(type: "bigint", nullable: false),
                    ProductMaterialCardId = table.Column<long>(type: "bigint", nullable: false),
                    PlannedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompletedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScrapQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    PlannedStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustomerOrderReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkCenter = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SupervisorUserId = table.Column<long>(type: "bigint", nullable: true),
                    PlannedSetupTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedRunTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualSetupTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualRunTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WorkOrderType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceReferenceId = table.Column<long>(type: "bigint", nullable: true),
                    ReleasedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleasedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CompletionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequiresQualityCheck = table.Column<bool>(type: "bit", nullable: false),
                    QualityStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_BillOfMaterials_BillOfMaterialId",
                        column: x => x.BillOfMaterialId,
                        principalTable: "BillOfMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrders_MaterialCards_ProductMaterialCardId",
                        column: x => x.ProductMaterialCardId,
                        principalTable: "MaterialCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_ReleasedByUserId",
                        column: x => x.ReleasedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_SupervisorUserId",
                        column: x => x.SupervisorUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionConfirmations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkOrderId = table.Column<long>(type: "bigint", nullable: false),
                    ConfirmationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConfirmationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScrapQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReworkQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OperatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    WorkCenter = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OperationSequence = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ConfirmationType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SetupTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RunTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DownTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DownTimeReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QualityStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QualityNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNumberFrom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNumberTo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActivityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WaitTime = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequiresQualityCheck = table.Column<bool>(type: "bit", nullable: false),
                    QualityCheckResult = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PostedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReversalReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaterialConsumed = table.Column<bool>(type: "bit", nullable: false),
                    RequiresStockPosting = table.Column<bool>(type: "bit", nullable: false),
                    StockPosted = table.Column<bool>(type: "bit", nullable: false),
                    StockPostingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ConfirmedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionConfirmations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionConfirmations_Users_ConfirmedByUserId",
                        column: x => x.ConfirmedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionConfirmations_Users_OperatorUserId",
                        column: x => x.OperatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionConfirmations_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillOfMaterialItems_BillOfMaterialId",
                table: "BillOfMaterialItems",
                column: "BillOfMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_BillOfMaterialItems_MaterialCardId",
                table: "BillOfMaterialItems",
                column: "MaterialCardId");

            migrationBuilder.CreateIndex(
                name: "IX_BillOfMaterialItems_SupplierMaterialCardId",
                table: "BillOfMaterialItems",
                column: "SupplierMaterialCardId");

            migrationBuilder.CreateIndex(
                name: "IX_BillOfMaterials_BOMCode",
                table: "BillOfMaterials",
                column: "BOMCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillOfMaterials_ProductMaterialCardId",
                table: "BillOfMaterials",
                column: "ProductMaterialCardId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionConfirmations_ConfirmationNumber",
                table: "ProductionConfirmations",
                column: "ConfirmationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionConfirmations_ConfirmedByUserId",
                table: "ProductionConfirmations",
                column: "ConfirmedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionConfirmations_OperatorUserId",
                table: "ProductionConfirmations",
                column: "OperatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionConfirmations_WorkOrderId",
                table: "ProductionConfirmations",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_BillOfMaterialId",
                table: "WorkOrders",
                column: "BillOfMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductMaterialCardId",
                table: "WorkOrders",
                column: "ProductMaterialCardId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ReleasedByUserId",
                table: "WorkOrders",
                column: "ReleasedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_SupervisorUserId",
                table: "WorkOrders",
                column: "SupervisorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_WorkOrderNumber",
                table: "WorkOrders",
                column: "WorkOrderNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillOfMaterialItems");

            migrationBuilder.DropTable(
                name: "ProductionConfirmations");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "BillOfMaterials");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Roles");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MaterialMovements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MaterialCategories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
