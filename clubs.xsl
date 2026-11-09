<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="utf-8" indent="yes"/>

  <!-- Виводить значення атрибута або "—", якщо атрибута немає -->
  <xsl:template name="attr">
    <xsl:param name="value"/>
    <xsl:choose>
      <xsl:when test="string($value) != ''"><xsl:value-of select="$value"/></xsl:when>
      <xsl:otherwise>—</xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/clubs">
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
        <table>
          <tr>
            <th>№</th><th>Назва</th><th>Тип</th><th>Факультет</th><th>Кафедра</th>
            <th>Курс</th><th>Тематика</th><th>День</th><th>Час</th>
            <th>Староста</th><th>Керівник</th>
          </tr>
          <xsl:apply-templates select="club"/>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="club">
    <tr>
      <td><xsl:value-of select="@id"/></td>
      <td><xsl:value-of select="@name"/></td>
      <td><xsl:call-template name="attr"><xsl:with-param name="value" select="@type"/></xsl:call-template></td>
      <td><xsl:value-of select="@faculty"/></td>
      <td><xsl:value-of select="@department"/></td>
      <td><xsl:call-template name="attr"><xsl:with-param name="value" select="@course"/></xsl:call-template></td>
      <td><xsl:call-template name="attr"><xsl:with-param name="value" select="@topic"/></xsl:call-template></td>
      <td><xsl:value-of select="meeting/@day"/></td>
      <td><xsl:value-of select="meeting/@time"/></td>
      <td><xsl:value-of select="starosta"/></td>
      <td><xsl:value-of select="leader"/></td>
    </tr>
  </xsl:template>
</xsl:stylesheet>