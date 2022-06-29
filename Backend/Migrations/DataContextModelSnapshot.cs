﻿// <auto-generated />
using System;
using Learnz.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Learnz.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Learnz.Entities.CreateQuestionDistribute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("CreateQuestionDistribute");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionDistributeAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LeftSide")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuestionDistributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RightSide")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionDistributeId");

                    b.ToTable("CreateQuestionDistributeAnswer");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMathematic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Digits")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("CreateQuestionMathematic");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMathematicVariable", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Digits")
                        .HasColumnType("int");

                    b.Property<string>("Display")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Interval")
                        .HasColumnType("float");

                    b.Property<double>("Max")
                        .HasColumnType("float");

                    b.Property<double>("Min")
                        .HasColumnType("float");

                    b.Property<Guid>("QuestionMathematicId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionMathematicId");

                    b.ToTable("CreateQuestionMathematicVariable");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMultipleChoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("CreateQuestionMultipleChoice");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMultipleChoiceAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsRight")
                        .HasColumnType("bit");

                    b.Property<Guid>("QuestionMultipleChoiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuestionMultipleChoiceId");

                    b.ToTable("CreateQuestionMultipleChoiceAnswer");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionOpenQuestion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("CreateQuestionOpenQuestion");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionTextField", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("CreateQuestionTextField");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionTrueFalse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Answer")
                        .HasColumnType("bit");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("CreateQuestionTrueFalse");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionWord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LanguageSubjectMain")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LanguageSubjectSecond")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SetId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("CreateQuestionWord");
                });

            modelBuilder.Entity("Learnz.Entities.CreateSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SetPolicy")
                        .HasColumnType("int");

                    b.Property<int>("SubjectMain")
                        .HasColumnType("int");

                    b.Property<int?>("SubjectSecond")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ModifiedById");

                    b.ToTable("CreateSet");
                });

            modelBuilder.Entity("Learnz.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProfileImageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("ProfileImageId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Learnz.Entities.GroupFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupFiles");
                });

            modelBuilder.Entity("Learnz.Entities.GroupMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("Learnz.Entities.GroupMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsInfoMessage")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SenderId");

                    b.ToTable("GroupMessages");
                });

            modelBuilder.Entity("Learnz.Entities.LearnzFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileNameExternal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileNameInternal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FilePolicy")
                        .HasColumnType("int");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ModifiedById");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Learnz.Entities.LearnzFileAnonymous", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileNameExternal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileNameInternal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FilesAnonymous");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherAsk", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("Answer")
                        .HasColumnType("bit");

                    b.Property<Guid>("AskedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("InterestedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AskedUserId");

                    b.HasIndex("InterestedUserId");

                    b.ToTable("TogetherAsks");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherConnection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId2")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId1");

                    b.HasIndex("UserId2");

                    b.ToTable("TogetherConnections");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("TogetherMessages");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherSwipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AskedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Choice")
                        .HasColumnType("bit");

                    b.Property<Guid>("SwiperUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AskedUserId");

                    b.HasIndex("SwiperUserId");

                    b.ToTable("TogetherSwipes");
                });

            modelBuilder.Entity("Learnz.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BadSubject1")
                        .HasColumnType("int");

                    b.Property<int>("BadSubject2")
                        .HasColumnType("int");

                    b.Property<int>("BadSubject3")
                        .HasColumnType("int");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GoodSubject1")
                        .HasColumnType("int");

                    b.Property<int>("GoodSubject2")
                        .HasColumnType("int");

                    b.Property<int>("GoodSubject3")
                        .HasColumnType("int");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("Information")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("ProfileImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("RefreshExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProfileImageId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionDistribute", b =>
                {
                    b.HasOne("Learnz.Entities.CreateSet", "Set")
                        .WithMany("QuestionDistributes")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionDistributeAnswer", b =>
                {
                    b.HasOne("Learnz.Entities.CreateQuestionDistribute", "QuestionDistribute")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionDistributeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("QuestionDistribute");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMathematic", b =>
                {
                    b.HasOne("Learnz.Entities.CreateSet", "Set")
                        .WithMany("QuestionMathematics")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMathematicVariable", b =>
                {
                    b.HasOne("Learnz.Entities.CreateQuestionMathematic", "QuestionMathematic")
                        .WithMany("Variables")
                        .HasForeignKey("QuestionMathematicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("QuestionMathematic");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMultipleChoice", b =>
                {
                    b.HasOne("Learnz.Entities.CreateSet", "Set")
                        .WithMany("QuestionMultipleChoices")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMultipleChoiceAnswer", b =>
                {
                    b.HasOne("Learnz.Entities.CreateQuestionMultipleChoice", "QuestionMultipleChoice")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionMultipleChoiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("QuestionMultipleChoice");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionOpenQuestion", b =>
                {
                    b.HasOne("Learnz.Entities.CreateSet", "Set")
                        .WithMany("QuestionOpenQuestions")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionTextField", b =>
                {
                    b.HasOne("Learnz.Entities.CreateSet", "Set")
                        .WithMany("QuestionTextFields")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionTrueFalse", b =>
                {
                    b.HasOne("Learnz.Entities.CreateSet", "Set")
                        .WithMany("QuestionTrueFalses")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionWord", b =>
                {
                    b.HasOne("Learnz.Entities.CreateSet", "Set")
                        .WithMany("QuestionWords")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Learnz.Entities.CreateSet", b =>
                {
                    b.HasOne("Learnz.Entities.User", "CreatedBy")
                        .WithMany("CreateSetCreated")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.User", "ModifiedBy")
                        .WithMany("CreateSetModified")
                        .HasForeignKey("ModifiedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("ModifiedBy");
                });

            modelBuilder.Entity("Learnz.Entities.Group", b =>
                {
                    b.HasOne("Learnz.Entities.User", "Admin")
                        .WithMany("GroupAdmin")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.LearnzFile", "ProfileImage")
                        .WithMany("GroupImageFiles")
                        .HasForeignKey("ProfileImageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("ProfileImage");
                });

            modelBuilder.Entity("Learnz.Entities.GroupFile", b =>
                {
                    b.HasOne("Learnz.Entities.LearnzFile", "File")
                        .WithMany("GroupFiles")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.Group", "Group")
                        .WithMany("GroupFiles")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Learnz.Entities.GroupMember", b =>
                {
                    b.HasOne("Learnz.Entities.Group", "Group")
                        .WithMany("GroupMembers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.User", "User")
                        .WithMany("GroupMembers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Learnz.Entities.GroupMessage", b =>
                {
                    b.HasOne("Learnz.Entities.Group", "Group")
                        .WithMany("GroupMessages")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.User", "Sender")
                        .WithMany("GroupMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Learnz.Entities.LearnzFile", b =>
                {
                    b.HasOne("Learnz.Entities.User", "CreatedBy")
                        .WithMany("LearnzFileCreated")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.User", "ModifiedBy")
                        .WithMany("LearnzFileModified")
                        .HasForeignKey("ModifiedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("ModifiedBy");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherAsk", b =>
                {
                    b.HasOne("Learnz.Entities.User", "AskedUser")
                        .WithMany("TogetherAskAskedUsers")
                        .HasForeignKey("AskedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.User", "InterestedUser")
                        .WithMany("TogetherAskInterestedUsers")
                        .HasForeignKey("InterestedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AskedUser");

                    b.Navigation("InterestedUser");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherConnection", b =>
                {
                    b.HasOne("Learnz.Entities.User", "User1")
                        .WithMany("TogetherConnectionUsers1")
                        .HasForeignKey("UserId1")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.User", "User2")
                        .WithMany("TogetherConnectionUsers2")
                        .HasForeignKey("UserId2")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User1");

                    b.Navigation("User2");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherMessage", b =>
                {
                    b.HasOne("Learnz.Entities.User", "Receiver")
                        .WithMany("TogetherMessageReceivers")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.User", "Sender")
                        .WithMany("TogetherMessageSenders")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherSwipe", b =>
                {
                    b.HasOne("Learnz.Entities.User", "AskedUser")
                        .WithMany("TogetherSwipeAskedUsers")
                        .HasForeignKey("AskedUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Learnz.Entities.User", "SwiperUser")
                        .WithMany("TogetherSwipeSwiperUsers")
                        .HasForeignKey("SwiperUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AskedUser");

                    b.Navigation("SwiperUser");
                });

            modelBuilder.Entity("Learnz.Entities.User", b =>
                {
                    b.HasOne("Learnz.Entities.LearnzFileAnonymous", "ProfileImage")
                        .WithMany("ProfileImages")
                        .HasForeignKey("ProfileImageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ProfileImage");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionDistribute", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMathematic", b =>
                {
                    b.Navigation("Variables");
                });

            modelBuilder.Entity("Learnz.Entities.CreateQuestionMultipleChoice", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Learnz.Entities.CreateSet", b =>
                {
                    b.Navigation("QuestionDistributes");

                    b.Navigation("QuestionMathematics");

                    b.Navigation("QuestionMultipleChoices");

                    b.Navigation("QuestionOpenQuestions");

                    b.Navigation("QuestionTextFields");

                    b.Navigation("QuestionTrueFalses");

                    b.Navigation("QuestionWords");
                });

            modelBuilder.Entity("Learnz.Entities.Group", b =>
                {
                    b.Navigation("GroupFiles");

                    b.Navigation("GroupMembers");

                    b.Navigation("GroupMessages");
                });

            modelBuilder.Entity("Learnz.Entities.LearnzFile", b =>
                {
                    b.Navigation("GroupFiles");

                    b.Navigation("GroupImageFiles");
                });

            modelBuilder.Entity("Learnz.Entities.LearnzFileAnonymous", b =>
                {
                    b.Navigation("ProfileImages");
                });

            modelBuilder.Entity("Learnz.Entities.User", b =>
                {
                    b.Navigation("CreateSetCreated");

                    b.Navigation("CreateSetModified");

                    b.Navigation("GroupAdmin");

                    b.Navigation("GroupMembers");

                    b.Navigation("GroupMessages");

                    b.Navigation("LearnzFileCreated");

                    b.Navigation("LearnzFileModified");

                    b.Navigation("TogetherAskAskedUsers");

                    b.Navigation("TogetherAskInterestedUsers");

                    b.Navigation("TogetherConnectionUsers1");

                    b.Navigation("TogetherConnectionUsers2");

                    b.Navigation("TogetherMessageReceivers");

                    b.Navigation("TogetherMessageSenders");

                    b.Navigation("TogetherSwipeAskedUsers");

                    b.Navigation("TogetherSwipeSwiperUsers");
                });
#pragma warning restore 612, 618
        }
    }
}