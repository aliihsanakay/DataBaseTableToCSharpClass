 SELECT    (CASE
                WHEN col.data_type = 'VARCHAR2'
                THEN
                   '[MaxLength(' || col.data_length || ')]' || CHR (10)
                WHEN col.data_type = 'VARCHAR'
                THEN
                   '[MaxLength(' || col.data_length || ')] ' || CHR (10)
                WHEN col.data_type = 'NCHAR'
                THEN
                   '[MaxLength(' || col.data_length || ')] ' || CHR (10)
                WHEN col.data_type = 'CHAR'
                THEN
                   '[MaxLength(' || col.data_length || ')] ' || CHR (10)
             END)
         || 'public '
         || (CASE
                WHEN col.data_type = 'VARCHAR2'
                THEN
                   ' string '
                WHEN col.data_type = 'VARCHAR'
                THEN
                   ' string '
                WHEN col.data_type = 'NCHAR'
                THEN
                   ' string '
                WHEN col.data_type = 'CHAR'
                THEN
                   ' string '
                WHEN col.data_type = 'DATE' AND col.nullable = 'Y'
                THEN
                   ' DateTime? '
                WHEN col.data_type = 'DATE' AND col.nullable = 'N'
                THEN
                   ' DateTime '
                WHEN col.data_type = 'NUMBER' AND col.nullable = 'N'
                THEN
                   ' decimal '
                WHEN col.data_type = 'NUMBER' AND col.nullable = 'Y'
                THEN
                   ' decimal? '
                WHEN     col.data_type = 'NUMBER'
                     AND col.nullable = 'Y'
                     AND col.data_length = 1
                THEN
                   ' bool? '
                WHEN     col.data_type = 'NUMBER'
                     AND col.nullable = 'N'
                     AND col.data_length = 1
                THEN
                   ' bool '
             END)
         || col.column_name
         || ' { get; set; } '
            AS Entity,
            'public '
         || (CASE
                WHEN col.data_type = 'VARCHAR2'
                THEN
                   ' string '
                WHEN col.data_type = 'VARCHAR'
                THEN
                   ' string '
                WHEN col.data_type = 'NCHAR'
                THEN
                   ' string '
                WHEN col.data_type = 'CHAR'
                THEN
                   ' string '
                WHEN col.data_type = 'DATE' AND col.nullable = 'Y'
                THEN
                   ' DateTime? '
                WHEN col.data_type = 'DATE' AND col.nullable = 'N'
                THEN
                   ' DateTime '
                WHEN col.data_type = 'NUMBER' AND col.nullable = 'N'
                THEN
                   ' decimal '
                WHEN col.data_type = 'NUMBER' AND col.nullable = 'Y'
                THEN
                   ' decimal? '
                WHEN     col.data_type = 'NUMBER'
                     AND col.nullable = 'Y'
                     AND col.data_length = 1
                THEN
                   ' bool? '
                WHEN     col.data_type = 'NUMBER'
                     AND col.nullable = 'N'
                     AND col.data_length = 1
                THEN
                   ' bool '
             END)
         || REPLACE (INITCAP (col.column_name), '_')
         || ' { get; set; } '
            AS Dto,
            REPLACE (INITCAP (col.column_name), '_')
         || '=x.'
         || col.column_name
         || ','
            AS SelectCode,
            '<dxi-validation-rule type="stringLength" [max]="'||col.data_length||'"  message="'
         || REPLACE (NLS_INITCAP (col.column_name, 'NLS_SORT = XTURKISH'),
                     '_',
                     ' ')
         || ' alanı en fazla '
         || col.data_length
         || ' karakter olabilir."></dxi-validation-rule>'
         || (CASE
                WHEN col.nullable = 'N'
                THEN
                      CHR (10)
                   || '  <dxi-validation-rule type="required" message="'
                   || REPLACE (
                         NLS_INITCAP (col.column_name, 'NLS_SORT = XTURKISH'),
                         '_',
                         ' ')
                   || ' alanı zorunludur."></dxi-validation-rule>'
             END)
            AS Validation
    FROM sys.all_tab_columns col
         INNER JOIN
         (SELECT owner, table_name FROM sys.all_tables
          UNION
          SELECT owner, view_name AS table_name FROM sys.all_views) t
            ON col.owner = t.owner AND col.table_name = t.table_name
   WHERE     col.table_name = :PAR_TABLO_ADI
         AND col.column_name NOT IN ('INSERT_USER',
                                     'INSERT_DATETIME',
                                     'UPDATE_USER',
                                     'UPDATE_DATETIME',
                                     'TAMIM_ID')
ORDER BY col.column_id
