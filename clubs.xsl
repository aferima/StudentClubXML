<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="utf-8" indent="yes"/>

	<xsl:param name="name" select="''"/>
	<xsl:param name="faculty" select="''"/>
	<xsl:param name="department" select="''"/>
	<xsl:param name="type" select="''"/>
	<xsl:param name="course" select="''"/>
	<xsl:param name="day" select="''"/>
	<xsl:param name="topic" select="''"/>
	<xsl:param name="leader" select="''"/>

	<xsl:variable name="upper" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ'"/>
	<xsl:variable name="lower" select="'abcdefghijklmnopqrstuvwxyzабвгґдеєжзиіїйклмнопрстуфхцчшщьюя'"/>

	<xsl:template name="attr">
		<xsl:param name="value"/>
		<xsl:choose>
			<xsl:when test="string($value) != ''">
				<xsl:value-of select="$value"/>
			</xsl:when>
			<xsl:otherwise>—</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/clubs">
		<xsl:variable name="found" select="club[
      ($name = '' or contains(translate(@name, $upper, $lower), translate($name, $upper, $lower)))
      and ($faculty = '' or translate(@faculty, $upper, $lower) = translate($faculty, $upper, $lower))
      and ($department = '' or translate(@department, $upper, $lower) = translate($department, $upper, $lower))
      and ($type = '' or translate(@type, $upper, $lower) = translate($type, $upper, $lower))
      and ($course = '' or translate(@course, $upper, $lower) = translate($course, $upper, $lower))
      and ($day = '' or translate(meeting/@day, $upper, $lower) = translate($day, $upper, $lower))
      and ($topic = '' or contains(translate(@topic, $upper, $lower), translate($topic, $upper, $lower)))
      and ($leader = '' or contains(translate(leader, $upper, $lower), translate($leader, $upper, $lower)))
    ]"/>

		<html>
			<head>
				<meta charset="utf-8"/>
				<title>Студентські гуртки та семінари</title>
				<style>
					body { font-family: Segoe UI, Arial, sans-serif; margin: 24px; }
					h1 { font-size: 22px; }
					table { border-collapse: collapse; width: 100%; }
					th, td { border: 1px solid #bbb; padding: 6px 10px; text-align: left; }
					th { background: #512bd4; color: #fff; }
					tr:nth-child(even) { background: #f3f0fb; }
				</style>
			</head>
			<body>
				<h1>Студентські гуртки та семінари</h1>

				<xsl:if test="$name != '' or $faculty != '' or $department != '' or $type != '' or $course != '' or $day != '' or $topic != '' or $leader != ''">
					<p>
						<b>Критерії пошуку:</b>
						<xsl:if test="$name != ''">
							назва містить «<xsl:value-of select="$name"/>»;
						</xsl:if>
						<xsl:if test="$faculty != ''">
							факультет «<xsl:value-of select="$faculty"/>»;
						</xsl:if>
						<xsl:if test="$department != ''">
							кафедра «<xsl:value-of select="$department"/>»;
						</xsl:if>
						<xsl:if test="$type != ''">
							тип «<xsl:value-of select="$type"/>»;
						</xsl:if>
						<xsl:if test="$course != ''">
							курс «<xsl:value-of select="$course"/>»;
						</xsl:if>
						<xsl:if test="$day != ''">
							день «<xsl:value-of select="$day"/>»;
						</xsl:if>
						<xsl:if test="$topic != ''">
							тематика містить «<xsl:value-of select="$topic"/>»;
						</xsl:if>
						<xsl:if test="$leader != ''">
							керівник містить «<xsl:value-of select="$leader"/>»;
						</xsl:if>
					</p>
				</xsl:if>

				<xsl:choose>
					<xsl:when test="count($found) = 0">
						<p>Нічого не знайдено.</p>
					</xsl:when>
					<xsl:otherwise>
						<p>
							Знайдено гуртків: <xsl:value-of select="count($found)"/>
						</p>
						<table>
							<tr>
								<th>№</th>
								<th>Назва</th>
								<th>Тип</th>
								<th>Факультет</th>
								<th>Кафедра</th>
								<th>Курс</th>
								<th>Тематика</th>
								<th>День</th>
								<th>Час</th>
								<th>Староста</th>
								<th>Керівник</th>
							</tr>
							<xsl:apply-templates select="$found"/>
						</table>
					</xsl:otherwise>
				</xsl:choose>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="club">
		<tr>
			<td>
				<xsl:value-of select="@id"/>
			</td>
			<td>
				<xsl:value-of select="@name"/>
			</td>
			<td>
				<xsl:call-template name="attr">
					<xsl:with-param name="value" select="@type"/>
				</xsl:call-template>
			</td>
			<td>
				<xsl:value-of select="@faculty"/>
			</td>
			<td>
				<xsl:value-of select="@department"/>
			</td>
			<td>
				<xsl:call-template name="attr">
					<xsl:with-param name="value" select="@course"/>
				</xsl:call-template>
			</td>
			<td>
				<xsl:call-template name="attr">
					<xsl:with-param name="value" select="@topic"/>
				</xsl:call-template>
			</td>
			<td>
				<xsl:value-of select="meeting/@day"/>
			</td>
			<td>
				<xsl:value-of select="meeting/@time"/>
			</td>
			<td>
				<xsl:value-of select="starosta"/>
			</td>
			<td>
				<xsl:value-of select="leader"/>
			</td>
		</tr>
	</xsl:template>
</xsl:stylesheet>
