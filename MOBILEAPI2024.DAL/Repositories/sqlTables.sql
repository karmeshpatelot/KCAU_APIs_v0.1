USE [Orange_HRMS_Developer_312]
GO
/****** Object:  Table [dbo].[Attendance_Events_KCA]    Script Date: 10/16/2024 4:58:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance_Events_KCA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RowId] [nvarchar](max) NULL,
	[ServerDateTime] [datetime] NULL,
	[Date_Time] [datetime] NULL,
	[Parameter] [nvarchar](50) NULL,
	[EventIndex] [nvarchar](50) NULL,
	[UserIdName] [nvarchar](100) NULL,
	[UserId] [nvarchar](50) NULL,
	[UserName] [nvarchar](100) NULL,
	[UserPhotoExists] [nvarchar](10) NULL,
	[UserGroupId] [nvarchar](50) NULL,
	[UserGroupName] [nvarchar](100) NULL,
	[DeviceId] [nvarchar](50) NULL,
	[DeviceName] [nvarchar](100) NULL,
	[EventTypeId] [nvarchar](50) NULL,
	[EventTypeCode] [nvarchar](50) NULL,
	[UserUpdateByDevice] [nvarchar](10) NULL,
	[Hint] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Campus_Attendance_KCA]    Script Date: 10/16/2024 4:58:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Campus_Attendance_KCA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Campus] [nvarchar](max) NULL,
	[In_Out_Time] [datetime] NULL,
	[ForDate] [datetime] NULL,
	[IO_Flag] [nvarchar](255) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KCA_OTP_VERIFY]    Script Date: 10/16/2024 4:58:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KCA_OTP_VERIFY](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[OTP] [nvarchar](max) NULL,
	[IsVerify] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lecture_Attendance_KCA]    Script Date: 10/16/2024 4:58:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lecture_Attendance_KCA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[LectureName] [nvarchar](max) NULL,
	[Campus] [nvarchar](max) NULL,
	[InOutTime] [datetime] NULL,
	[ForDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff_KCA]    Script Date: 10/16/2024 4:58:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff_KCA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[sn] [varchar](50) NULL,
	[staff_no] [varchar](50) NULL,
	[first_name] [varchar](100) NULL,
	[middle_name] [varchar](100) NULL,
	[last_name] [varchar](100) NULL,
	[campus] [varchar](50) NULL,
	[date_joined] [date] NULL,
	[department] [varchar](100) NULL,
	[job_title] [varchar](100) NULL,
	[Gender] [varchar](10) NULL,
	[Allow] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students_KCA]    Script Date: 10/16/2024 4:58:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students_KCA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[sn] [varchar](255) NULL,
	[student_no] [varchar](255) NULL,
	[name] [varchar](255) NULL,
	[campus] [varchar](255) NULL,
	[date_registered] [varchar](255) NULL,
	[intake_period] [varchar](255) NULL,
	[dob] [varchar](255) NULL,
	[Gender] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[Password] [varchar](255) NULL,
	[Allow] [varchar](255) NULL,
	[Bal] [decimal](18, 2) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_InOut_Records_KCA]    Script Date: 10/16/2024 4:58:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_InOut_Records_KCA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[EnrollNo] [nvarchar](max) NULL,
	[LocationType] [nvarchar](max) NULL,
	[ForDate] [datetime] NULL,
	[InTime] [datetime] NULL,
	[OutTime] [datetime] NULL,
	[Duration] [nvarchar](max) NULL,
	[IpAddress] [nvarchar](max) NULL,
	[DeviceId] [nvarchar](max) NULL,
	[FeesStatus] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK__User_InO__3214EC07BE8B1344] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


USE [Orange_HRMS_Developer_312]
GO

/****** Object:  Table [dbo].[Login_Report_KCA]    Script Date: 10/23/2024 12:09:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Login_Report_KCA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[LoginTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [Orange_HRMS_Developer_312]
GO

/****** Object:  Table [dbo].[Admin_Users_KCA]    Script Date: 10/25/2024 3:26:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Admin_Users_KCA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AdminName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Status] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [Orange_HRMS_Developer_312]
GO

/****** Object:  Table [dbo].[Device_Configuration_KCA]    Script Date: 10/25/2024 3:27:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Device_Configuration_KCA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [nvarchar](max) NULL,
	[Location] [nvarchar](max) NULL,
	[CampusName] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

