using ApiPermissions;
using ApiPermissions;
using Path = ApiPermissions.PathConstraints;

namespace oAuthTests
{
    public class UnitTest1
    {
        [Fact]
        public void NegativePermissionMatchForMissingResource()
        {
            var permissionsDocument = new PermissionsDocument();

            var authZChecker = new AuthZChecker();

            authZChecker.Load(permissionsDocument);
            var canAccess = authZChecker.CanAccess("/foo", "GET", "DelegatedPersonal",new string[] { "Foo.Read" } );
            
            Assert.Equal(AccessRequestResult.MissingResource, canAccess); 

        }

        [Fact]
        public void NegativePermissionMatchForMissingMethod()
        {
            var permissionsDocument = new PermissionsDocument();

            var fooRead = new Permission
            {
                PathSets = {
                        new PathSet() {
                            Paths = {
                                { "/foo",  null }
                            }
                        }
                    }
            };
            permissionsDocument.Permissions.Add("Foo.Read",fooRead);
            var authZChecker = new AuthZChecker();

            authZChecker.Load(permissionsDocument);
            var canAccess = authZChecker.CanAccess("/foo", "GET", "DelegatedPersonal", new string[] { "Foo.Read" });

            Assert.Equal(AccessRequestResult.UnsupportedMethod, canAccess);

        }

        [Fact]
        public void NegativePermissionMatchForMissingScheme()
        {
            var permissionsDocument = new PermissionsDocument();

            var fooRead = new Permission
            {
                PathSets = {
                        new PathSet() {
                            Methods = { 
                                "GET"
                            },
                            Paths = {
                                { "/foo",  null }
                            }
                        }
                    }
            };
            permissionsDocument.Permissions.Add("Foo.Read", fooRead);
            var authZChecker = new AuthZChecker();

            authZChecker.Load(permissionsDocument);
            var canAccess = authZChecker.CanAccess("/foo", "GET", "DelegatedPersonal", new string[] { "Foo.Read" });

            Assert.Equal(AccessRequestResult.UnsupportedScheme, canAccess);

        }

        [Fact]
        public void NegativePermissionMatchForInsufficientPermissions()
        {
            PermissionsDocument permissionsDocument = CreatePermissionsDocument();

            var authZChecker = new AuthZChecker();

            authZChecker.Load(permissionsDocument);
            var canAccess = authZChecker.CanAccess("/foo", "GET", "DelegatedPersonal", new string[] { "Bar.Read" });

            Assert.Equal(AccessRequestResult.InsufficientPermissions, canAccess);
        }

        private static PermissionsDocument CreatePermissionsDocument()
        {
            var permissionsDocument = new PermissionsDocument();

            var fooRead = new Permission
            {
                PathSets = {
                        new PathSet() {
                            Methods = {
                                "GET"
                            },
                            Schemes = {
                                "DelegatedPersonal"
                            },
                            Paths = {
                                { "/foo",  null }
                            }
                        }
                    }
            };
            permissionsDocument.Permissions.Add("Foo.Read", fooRead);
            return permissionsDocument;
        }

        [Fact]
        public void PositivePermissionMatch()
        {
            var permissionsDocument = new PermissionsDocument();

            var fooRead = new Permission
            {
                PathSets = {
                        new PathSet() {
                            Methods = {
                                "GET"
                            },
                            Schemes = {
                                "DelegatedPersonal"
                            },
                            Paths = {
                                { "/foo",  null }
                            }
                        }
                    }
            };
            permissionsDocument.Permissions.Add("Foo.Read", fooRead);
            var authZChecker = new AuthZChecker();

            authZChecker.Load(permissionsDocument);
            var canAccess = authZChecker.CanAccess("/foo", "GET", "DelegatedPersonal", new string[] { "Foo.Read" });

            Assert.Equal(AccessRequestResult.Success, canAccess);
        }
    }
}