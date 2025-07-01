using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teklas_Intern_ERP.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DetailedERPEntitiesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "WorkOrders",
                newName: "WorkOrderNo");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "WorkOrders",
                newName: "PlannedStartDate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Warehouses",
                newName: "WarehouseName");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Warehouses",
                newName: "WarehouseCode");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SupplierTypes",
                newName: "TypeName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Suppliers",
                newName: "TaxOffice");

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "PurchaseOrders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "PurchaseOrders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "MaterialCardId",
                table: "MaterialMovements",
                newName: "MaterialId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "MaterialMovements",
                newName: "MovementDate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MaterialCategories",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "MaterialCards",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "MaterialCards",
                newName: "UnitOfMeasure");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Locations",
                newName: "LocationName");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Locations",
                newName: "LocationCode");

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "CustomerOrders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "CustomerOrders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BillOfMaterials",
                newName: "UnitOfMeasure");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualEndDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualStartDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedEndDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "WorkOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "WorkOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePerson",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Capacity",
                table: "Warehouses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Warehouses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Warehouses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResponsiblePerson",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Warehouses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "SupplierTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SupplierTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SupplierTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "SupplierTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Suppliers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupplierCode",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaxNumber",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Suppliers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "StockEntries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "BatchNo",
                table: "StockEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StockEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StockEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "StockEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "StockEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LotNo",
                table: "StockEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "StockEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                table: "StockEntries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "StockEntries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "StockEntries",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentTerms",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrderNo",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "PurchaseOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfirmedBy",
                table: "ProductionConfirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ConfirmedQuantity",
                table: "ProductionConfirmations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductionConfirmations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductionConfirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ScrapQuantity",
                table: "ProductionConfirmations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "MaterialMovements",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MaterialMovements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MaterialMovements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MaterialMovements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DestinationWarehouseId",
                table: "MaterialMovements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MovementType",
                table: "MaterialMovements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceDocumentNo",
                table: "MaterialMovements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SourceWarehouseId",
                table: "MaterialMovements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "MaterialMovements",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "MaterialMovements",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CategoryCode",
                table: "MaterialCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "MaterialCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MaterialCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MaterialCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "MaterialCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "MaterialCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "MaterialCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MaterialCards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MaterialCards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Length",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterialCode",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterialName",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumStockLevel",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumStockLevel",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginCountry",
                table: "MaterialCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReorderLevel",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalesPrice",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ShelfLife",
                table: "MaterialCards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "MaterialCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Volume",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Width",
                table: "MaterialCards",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Capacity",
                table: "Locations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Locations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Locations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CustomerOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CustomerOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "CustomerOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "CustomerOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "CustomerOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "CustomerOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OrderNo",
                table: "CustomerOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentTerms",
                table: "CustomerOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "CustomerOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CustomerOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BillOfMaterials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "BillOfMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "BillOfMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "BillOfMaterials",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ScrapRate",
                table: "BillOfMaterials",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "BillOfMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "BillOfMaterials",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualEndDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ActualStartDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "PlannedEndDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ResponsiblePerson",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "ResponsiblePerson",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "SupplierTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SupplierTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SupplierTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "SupplierTypes");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "SupplierCode",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "TaxNumber",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "BatchNo",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "LotNo",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "StockEntries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PaymentTerms",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderNo",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "ConfirmedBy",
                table: "ProductionConfirmations");

            migrationBuilder.DropColumn(
                name: "ConfirmedQuantity",
                table: "ProductionConfirmations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProductionConfirmations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductionConfirmations");

            migrationBuilder.DropColumn(
                name: "ScrapQuantity",
                table: "ProductionConfirmations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MaterialMovements");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MaterialMovements");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MaterialMovements");

            migrationBuilder.DropColumn(
                name: "DestinationWarehouseId",
                table: "MaterialMovements");

            migrationBuilder.DropColumn(
                name: "MovementType",
                table: "MaterialMovements");

            migrationBuilder.DropColumn(
                name: "ReferenceDocumentNo",
                table: "MaterialMovements");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "MaterialMovements");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "MaterialMovements");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "MaterialMovements");

            migrationBuilder.DropColumn(
                name: "CategoryCode",
                table: "MaterialCategories");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "MaterialCategories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MaterialCategories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MaterialCategories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "MaterialCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "MaterialCategories");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "MaterialCode",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "MaterialName",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "MaximumStockLevel",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "MinimumStockLevel",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "OriginCountry",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "ReorderLevel",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "SalesPrice",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "ShelfLife",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "MaterialCards");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "PaymentTerms",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BillOfMaterials");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "BillOfMaterials");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BillOfMaterials");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BillOfMaterials");

            migrationBuilder.DropColumn(
                name: "ScrapRate",
                table: "BillOfMaterials");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "BillOfMaterials");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "BillOfMaterials");

            migrationBuilder.RenameColumn(
                name: "WorkOrderNo",
                table: "WorkOrders",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "PlannedStartDate",
                table: "WorkOrders",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "WarehouseName",
                table: "Warehouses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "WarehouseCode",
                table: "Warehouses",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "TypeName",
                table: "SupplierTypes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TaxOffice",
                table: "Suppliers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "PurchaseOrders",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "PurchaseOrders",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "MovementDate",
                table: "MaterialMovements",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "MaterialMovements",
                newName: "MaterialCardId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MaterialCategories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "MaterialCards",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "MaterialCards",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "LocationName",
                table: "Locations",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "LocationCode",
                table: "Locations",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "CustomerOrders",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "CustomerOrders",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasure",
                table: "BillOfMaterials",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "StockEntries",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "MaterialMovements",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
