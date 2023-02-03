using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace web_api_managemen_user.Migrations
{
    public partial class _15_09_2022_model_user_management_db_v_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_action_feature",
                columns: table => new
                {
                    m_action_feature_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    m_action_feature_name = table.Column<string>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true),
                    status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_action_feature", x => x.m_action_feature_id);
                });

            migrationBuilder.CreateTable(
                name: "m_feature",
                columns: table => new
                {
                    m_feature_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    m_feature_name = table.Column<string>(nullable: false),
                    index = table.Column<int>(nullable: true),
                    name_link_feature = table.Column<string>(nullable: true),
                    status = table.Column<bool>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true),
                    feature_icon = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_feature", x => x.m_feature_id);
                });

            migrationBuilder.CreateTable(
                name: "m_group_feature",
                columns: table => new
                {
                    m_group_feature_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<bool>(nullable: false),
                    index = table.Column<int>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true),
                    group_feature_name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_group_feature", x => x.m_group_feature_id);
                });

            migrationBuilder.CreateTable(
                name: "m_group_user",
                columns: table => new
                {
                    m_group_user_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    m_group_user_name = table.Column<string>(nullable: false),
                    status = table.Column<bool>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_group_user", x => x.m_group_user_id);
                });

            migrationBuilder.CreateTable(
                name: "m_kelurahan_desa",
                columns: table => new
                {
                    m_kelurahan_desa_id = table.Column<string>(nullable: false),
                    nama_kelurahan_desa = table.Column<string>(maxLength: 50, nullable: false),
                    nama_kecamatan = table.Column<string>(maxLength: 50, nullable: false),
                    nama_kabupaten_kota = table.Column<string>(maxLength: 50, nullable: false),
                    jenis_kabupaten_kota = table.Column<string>(maxLength: 10, nullable: false),
                    nama_propinsi = table.Column<string>(maxLength: 50, nullable: false),
                    kode_pos = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_kelurahan_desa", x => x.m_kelurahan_desa_id);
                });

            migrationBuilder.CreateTable(
                name: "m_organisasi",
                columns: table => new
                {
                    m_organisasi_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nama_oganisasi = table.Column<string>(nullable: false),
                    alamat_organisasi = table.Column<string>(nullable: true),
                    status_aktif = table.Column<bool>(nullable: false),
                    email_organisasi = table.Column<string>(nullable: true),
                    website_organisasi = table.Column<string>(nullable: true),
                    telepon_organisasi = table.Column<string>(nullable: true),
                    path_logo_organiasi = table.Column<string>(nullable: true),
                    npwp_organiasi = table.Column<string>(nullable: true),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_organisasi", x => x.m_organisasi_id);
                });

            migrationBuilder.CreateTable(
                name: "m_project_application",
                columns: table => new
                {
                    m_project_application_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    m_project_application_name = table.Column<string>(nullable: false),
                    status_aktif = table.Column<bool>(nullable: false),
                    key_project = table.Column<string>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true),
                    type_project = table.Column<string>(nullable: true),
                    scope_fitur_project = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_project_application", x => x.m_project_application_id);
                });

            migrationBuilder.CreateTable(
                name: "m_user",
                columns: table => new
                {
                    m_user_id = table.Column<Guid>(nullable: false),
                    m_username = table.Column<string>(nullable: false),
                    m_user_password = table.Column<string>(nullable: false),
                    m_user_email = table.Column<string>(nullable: false),
                    status = table.Column<bool>(nullable: false),
                    status_aktif = table.Column<string>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_user", x => x.m_user_id);
                });

            migrationBuilder.CreateTable(
                name: "m_lokasi",
                columns: table => new
                {
                    m_lokasi_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    m_organisasi_id = table.Column<int>(nullable: false),
                    parent_lokasi_id = table.Column<int>(nullable: false),
                    parent_index = table.Column<int>(nullable: false),
                    nama_lokasi = table.Column<string>(nullable: false),
                    kode_lokasi = table.Column<string>(nullable: false),
                    provinsi = table.Column<string>(nullable: false),
                    kabupaten = table.Column<string>(nullable: false),
                    kecamatan = table.Column<string>(nullable: false),
                    kelurahan = table.Column<string>(nullable: false),
                    alamat = table.Column<string>(nullable: false),
                    status_aktif = table.Column<bool>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true),
                    name_location_secondary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_lokasi", x => x.m_lokasi_id);
                    table.ForeignKey(
                        name: "FK_m_lokasi_m_organisasi_m_organisasi_id",
                        column: x => x.m_organisasi_id,
                        principalTable: "m_organisasi",
                        principalColumn: "m_organisasi_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "map_feature_group_project",
                columns: table => new
                {
                    map_feature_group_project_id = table.Column<Guid>(nullable: false),
                    m_project_application_id = table.Column<int>(nullable: false),
                    status_aktif = table.Column<bool>(nullable: false),
                    m_group_feature_id = table.Column<int>(nullable: false),
                    m_feature_id = table.Column<int>(nullable: false),
                    feature_icon = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_feature_group_project", x => x.map_feature_group_project_id);
                    table.ForeignKey(
                        name: "FK_map_feature_group_project_m_feature_m_feature_id",
                        column: x => x.m_feature_id,
                        principalTable: "m_feature",
                        principalColumn: "m_feature_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_map_feature_group_project_m_group_feature_m_group_feature_id",
                        column: x => x.m_group_feature_id,
                        principalTable: "m_group_feature",
                        principalColumn: "m_group_feature_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_map_feature_group_project_m_project_application_m_project_a~",
                        column: x => x.m_project_application_id,
                        principalTable: "m_project_application",
                        principalColumn: "m_project_application_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "map_group_user",
                columns: table => new
                {
                    map_group_user_id = table.Column<Guid>(nullable: false),
                    status_aktif = table.Column<bool>(nullable: false),
                    m_group_user_id = table.Column<int>(nullable: false),
                    m_project_application_id = table.Column<int>(nullable: false),
                    m_user_id = table.Column<Guid>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_group_user", x => x.map_group_user_id);
                    table.ForeignKey(
                        name: "FK_map_group_user_m_group_user_m_group_user_id",
                        column: x => x.m_group_user_id,
                        principalTable: "m_group_user",
                        principalColumn: "m_group_user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_map_group_user_m_project_application_m_project_application_~",
                        column: x => x.m_project_application_id,
                        principalTable: "m_project_application",
                        principalColumn: "m_project_application_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_map_group_user_m_user_m_user_id",
                        column: x => x.m_user_id,
                        principalTable: "m_user",
                        principalColumn: "m_user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "map_user_lokasi",
                columns: table => new
                {
                    map_user_lokasi_id = table.Column<Guid>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true),
                    m_user_id = table.Column<Guid>(nullable: false),
                    m_project_application_id = table.Column<int>(nullable: false),
                    m_lokasi_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_user_lokasi", x => x.map_user_lokasi_id);
                    table.ForeignKey(
                        name: "FK_map_user_lokasi_m_lokasi_m_lokasi_id",
                        column: x => x.m_lokasi_id,
                        principalTable: "m_lokasi",
                        principalColumn: "m_lokasi_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_map_user_lokasi_m_project_application_m_project_application~",
                        column: x => x.m_project_application_id,
                        principalTable: "m_project_application",
                        principalColumn: "m_project_application_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_map_user_lokasi_m_user_m_user_id",
                        column: x => x.m_user_id,
                        principalTable: "m_user",
                        principalColumn: "m_user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "map_feature_group",
                columns: table => new
                {
                    map_feature_group_id = table.Column<Guid>(nullable: false),
                    m_group_user_id = table.Column<int>(nullable: false),
                    action_feature = table.Column<string>(nullable: false),
                    create_at = table.Column<DateTime>(nullable: true),
                    create_by = table.Column<string>(nullable: true),
                    update_at = table.Column<DateTime>(nullable: true),
                    update_by = table.Column<string>(nullable: true),
                    map_feature_group_project_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_feature_group", x => x.map_feature_group_id);
                    table.ForeignKey(
                        name: "FK_map_feature_group_m_group_user_m_group_user_id",
                        column: x => x.m_group_user_id,
                        principalTable: "m_group_user",
                        principalColumn: "m_group_user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_map_feature_group_map_feature_group_project_map_feature_gro~",
                        column: x => x.map_feature_group_project_id,
                        principalTable: "map_feature_group_project",
                        principalColumn: "map_feature_group_project_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_m_action_feature_m_action_feature_name",
                table: "m_action_feature",
                column: "m_action_feature_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_m_lokasi_m_organisasi_id",
                table: "m_lokasi",
                column: "m_organisasi_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_feature_group_m_group_user_id",
                table: "map_feature_group",
                column: "m_group_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_feature_group_map_feature_group_project_id",
                table: "map_feature_group",
                column: "map_feature_group_project_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_feature_group_project_m_feature_id",
                table: "map_feature_group_project",
                column: "m_feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_feature_group_project_m_group_feature_id",
                table: "map_feature_group_project",
                column: "m_group_feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_feature_group_project_m_project_application_id",
                table: "map_feature_group_project",
                column: "m_project_application_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_group_user_m_group_user_id",
                table: "map_group_user",
                column: "m_group_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_group_user_m_project_application_id",
                table: "map_group_user",
                column: "m_project_application_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_group_user_m_user_id",
                table: "map_group_user",
                column: "m_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_user_lokasi_m_lokasi_id",
                table: "map_user_lokasi",
                column: "m_lokasi_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_user_lokasi_m_project_application_id",
                table: "map_user_lokasi",
                column: "m_project_application_id");

            migrationBuilder.CreateIndex(
                name: "IX_map_user_lokasi_m_user_id",
                table: "map_user_lokasi",
                column: "m_user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_action_feature");

            migrationBuilder.DropTable(
                name: "m_kelurahan_desa");

            migrationBuilder.DropTable(
                name: "map_feature_group");

            migrationBuilder.DropTable(
                name: "map_group_user");

            migrationBuilder.DropTable(
                name: "map_user_lokasi");

            migrationBuilder.DropTable(
                name: "map_feature_group_project");

            migrationBuilder.DropTable(
                name: "m_group_user");

            migrationBuilder.DropTable(
                name: "m_lokasi");

            migrationBuilder.DropTable(
                name: "m_user");

            migrationBuilder.DropTable(
                name: "m_feature");

            migrationBuilder.DropTable(
                name: "m_group_feature");

            migrationBuilder.DropTable(
                name: "m_project_application");

            migrationBuilder.DropTable(
                name: "m_organisasi");
        }
    }
}
