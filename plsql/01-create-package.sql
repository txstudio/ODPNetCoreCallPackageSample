/* CREATE PACKAGE SPEC */
CREATE OR REPLACE PACKAGE PKG_PRODUCT
AS
    FUNCTION TOTALCOUNT
        RETURN INT;
        
    PROCEDURE QUERY_PRODUCT (
        P_CODE		IN  PRODUCT.CODE%TYPE,
        P_NAME		IN  PRODUCT.NAME%TYPE,
        O_COUNT		OUT SYS_REFCURSOR,
        O_RETURN	OUT SYS_REFCURSOR
    );
    
    PROCEDURE GET_PRODUCT (
        P_ID		IN  PRODUCT.ID%TYPE,
        O_RETURN	OUT SYS_REFCURSOR
    );
    
    PROCEDURE CREATE_PRODUCT (
        P_CODE		IN  PRODUCT.CODE%TYPE,
        P_NAME		IN  PRODUCT.NAME%TYPE,
        O_ID  		OUT PRODUCT.ID%TYPE
    );
    
    PROCEDURE UPDATE_PRODUCT (
        P_CODE		IN  PRODUCT.CODE%TYPE,
        P_NAME		IN  PRODUCT.NAME%TYPE,
        P_ID  		IN  PRODUCT.ID%TYPE,
        O_RESULT	OUT CHAR
    );
    
    PROCEDURE DELETE_PRODUCT (
        P_ID		IN  PRODUCT.ID%TYPE,
        O_RESULT	OUT CHAR
    );
        
END PKG_PRODUCT;

/* CREATE PACKAGE BODY */
CREATE OR REPLACE PACKAGE BODY PKG_PRODUCT
AS
    FUNCTION TOTALCOUNT
    RETURN INT
    IS
        PRODUCT_COUNT INT;
    BEGIN
        SELECT COUNT(*) INTO PRODUCT_COUNT
        FROM PRODUCT;
        
        RETURN PRODUCT_COUNT;
    END TOTALCOUNT;
        
    PROCEDURE QUERY_PRODUCT (
        P_CODE  	IN  PRODUCT.CODE%TYPE,
        P_NAME  	IN  PRODUCT.NAME%TYPE,
        O_COUNT 	OUT SYS_REFCURSOR,
        O_RETURN	OUT SYS_REFCURSOR
    )
    AS
    BEGIN                
        OPEN O_COUNT FOR
            SELECT COUNT(ID)
            FROM PRODUCT
            WHERE ((CODE LIKE '%'||P_CODE||'%') OR (P_CODE IS NULL))
                AND ((NAME LIKE '%'||P_NAME||'%') OR (P_NAME IS NULL)); 
                
        OPEN O_RETURN FOR
            SELECT ID
                ,CODE
                ,NAME 
            FROM PRODUCT
            WHERE ((CODE LIKE '%'||P_CODE||'%') OR (P_CODE IS NULL))
                AND ((NAME LIKE '%'||P_NAME||'%') OR (P_NAME IS NULL));      
    
    END QUERY_PRODUCT;
    
    PROCEDURE GET_PRODUCT (
        P_ID      	IN  PRODUCT.ID%TYPE,
        O_RETURN  	OUT SYS_REFCURSOR
    )
    IS
    BEGIN
        OPEN O_RETURN FOR
            SELECT ID,CODE,NAME
            FROM PRODUCT
            WHERE ID = P_ID;
    END GET_PRODUCT;
    
    PROCEDURE CREATE_PRODUCT (
        P_CODE    	IN  PRODUCT.CODE%TYPE,
        P_NAME    	IN  PRODUCT.NAME%TYPE,
        O_ID      	OUT PRODUCT.ID%TYPE
    )
    AS
    BEGIN
        O_ID := SEQ_PRODUCT.NEXTVAL;
        
        INSERT INTO PRODUCT (ID,CODE,NAME)
            VALUES (O_ID,P_CODE,P_NAME);
            
    END CREATE_PRODUCT;
    
    PROCEDURE UPDATE_PRODUCT (
        P_CODE      IN  PRODUCT.CODE%TYPE,
        P_NAME      IN  PRODUCT.NAME%TYPE,
        P_ID        IN  PRODUCT.ID%TYPE,
        O_RESULT    OUT CHAR
    )
    AS
    BEGIN
        O_RESULT := 'N';
        
        UPDATE PRODUCT
            SET CODE = P_CODE,
                NAME = P_NAME
        WHERE ID = P_ID;
        
        O_RESULT := 'Y';
    
    END UPDATE_PRODUCT;
    
    PROCEDURE DELETE_PRODUCT (
        P_ID      IN  PRODUCT.ID%TYPE,
        O_RESULT  OUT CHAR
    )
    AS
    BEGIN
    
        O_RESULT := 'N';
        
        DELETE PRODUCT
            WHERE ID = P_ID;
        
        O_RESULT := 'Y';
    
    END DELETE_PRODUCT;
        
END PKG_PRODUCT;