import { BrowserRouter, Route, Routes } from "react-router-dom";
import { AuthProvider } from "./contexts/AuthContext";
import ProtectedRoute from "./components/ProtectedRoute";
import HomePageOne from "./pages/HomePageOne";
import HomePageTwo from "./pages/HomePageTwo";
import HomePageThree from "./pages/HomePageThree";
import HomePageFour from "./pages/HomePageFour";
import HomePageFive from "./pages/HomePageFive";
import HomePageSix from "./pages/HomePageSix";
import HomePageSeven from "./pages/HomePageSeven";
import EmailPage from "./pages/EmailPage";
import AddUserPage from "./pages/AddUserPage";
import AlertPage from "./pages/AlertPage";
import AssignRolePage from "./pages/AssignRolePage";
import AvatarPage from "./pages/AvatarPage";
import BadgesPage from "./pages/BadgesPage";
import ButtonPage from "./pages/ButtonPage";
import CalendarMainPage from "./pages/CalendarMainPage";
import CardPage from "./pages/CardPage";
import CarouselPage from "./pages/CarouselPage";
import ChatEmptyPage from "./pages/ChatEmptyPage";
import ChatMessagePage from "./pages/ChatMessagePage";
import ChatProfilePage from "./pages/ChatProfilePage";
import CodeGeneratorNewPage from "./pages/CodeGeneratorNewPage";
import CodeGeneratorPage from "./pages/CodeGeneratorPage";
import ColorsPage from "./pages/ColorsPage";
import ColumnChartPage from "./pages/ColumnChartPage";
import CompanyPage from "./pages/CompanyPage";
import CurrenciesPage from "./pages/CurrenciesPage";
import DropdownPage from "./pages/DropdownPage";
import ErrorPage from "./pages/ErrorPage";
import FaqPage from "./pages/FaqPage";
import ForgotPasswordPage from "./pages/ForgotPasswordPage";
import FormLayoutPage from "./pages/FormLayoutPage";
import FormValidationPage from "./pages/FormValidationPage";
import FormPage from "./pages/FormPage";
import GalleryPage from "./pages/GalleryPage";
import ImageGeneratorPage from "./pages/ImageGeneratorPage";
import ImageUploadPage from "./pages/ImageUploadPage";
import InvoiceAddPage from "./pages/InvoiceAddPage";
import InvoiceEditPage from "./pages/InvoiceEditPage";
import InvoiceListPage from "./pages/InvoiceListPage";
import InvoicePreviewPage from "./pages/InvoicePreviewPage";
import KanbanPage from "./pages/KanbanPage";
import LanguagePage from "./pages/LanguagePage";
import LineChartPage from "./pages/LineChartPage";
import ListPage from "./pages/ListPage";
import MarketplaceDetailsPage from "./pages/MarketplaceDetailsPage";
import MarketplacePage from "./pages/MarketplacePage";
import NotificationAlertPage from "./pages/NotificationAlertPage";
import NotificationPage from "./pages/NotificationPage";
import PaginationPage from "./pages/PaginationPage";
import PaymentGatewayPage from "./pages/PaymentGatewayPage";
import PieChartPage from "./pages/PieChartPage";
import PortfolioPage from "./pages/PortfolioPage";
import PricingPage from "./pages/PricingPage";
import ProgressPage from "./pages/ProgressPage";
import RadioPage from "./pages/RadioPage";
import RoleAccessPage from "./pages/RoleAccessPage";
import SignInPage from "./pages/SignInPage";
import SignUpPage from "./pages/SignUpPage";
import StarRatingPage from "./pages/StarRatingPage";
import StarredPage from "./pages/StarredPage";
import SwitchPage from "./pages/SwitchPage";
import TableBasicPage from "./pages/TableBasicPage";
import TableDataPage from "./pages/TableDataPage";
import TabsPage from "./pages/TabsPage";
import TagsPage from "./pages/TagsPage";
import TermsConditionPage from "./pages/TermsConditionPage";
import TextGeneratorPage from "./pages/TextGeneratorPage";
import ThemePage from "./pages/ThemePage";
import TooltipPage from "./pages/TooltipPage";
import TypographyPage from "./pages/TypographyPage";
import UsersGridPage from "./pages/UsersGridPage";
import UsersListPage from "./pages/UsersListPage";
import ViewDetailsPage from "./pages/ViewDetailsPage";
import VideoGeneratorPage from "./pages/VideoGeneratorPage";
import VideosPage from "./pages/VideosPage";
import ViewProfilePage from "./pages/ViewProfilePage";
import VoiceGeneratorPage from "./pages/VoiceGeneratorPage";
import WalletPage from "./pages/WalletPage";
import WidgetsPage from "./pages/WidgetsPage";
import WizardPage from "./pages/WizardPage";
import RouteScrollToTop from "./helper/RouteScrollToTop";
import TextGeneratorNewPage from "./pages/TextGeneratorNewPage";
import HomePageEight from "./pages/HomePageEight";
import HomePageNine from "./pages/HomePageNine";
import HomePageTen from "./pages/HomePageTen";
import HomePageEleven from "./pages/HomePageEleven";
import GalleryGridPage from "./pages/GalleryGridPage";
import GalleryMasonryPage from "./pages/GalleryMasonryPage";
import GalleryHoverPage from "./pages/GalleryHoverPage";
import BlogPage from "./pages/BlogPage";
import BlogDetailsPage from "./pages/BlogDetailsPage";
import AddBlogPage from "./pages/AddBlogPage";
import TestimonialsPage from "./pages/TestimonialsPage";
import ComingSoonPage from "./pages/ComingSoonPage";
import AccessDeniedPage from "./pages/AccessDeniedPage";
import MaintenancePage from "./pages/MaintenancePage";
import BlankPagePage from "./pages/BlankPagePage";
import MaterialCardPage from "./pages/MaterialCardPage";
import MaterialCardTrashPage from './pages/MaterialCardTrashPage';
import MaterialCategoryPage from './pages/MaterialCategoryPage';
import MaterialCategoryTrashPage from './pages/MaterialCategoryTrashPage';
import MaterialMovementPage from './pages/MaterialMovementPage';
import MaterialMovementTrashPage from './pages/MaterialMovementTrashPage';
import CreateRolePage from './pages/CreateRolePage';
import WarehousePage from './pages/WarehousePage';
import WarehouseTrashPage from './pages/WarehouseTrashPage';
import SupplierPage from './pages/SupplierPage';
import SupplierTrashPage from './pages/SupplierTrashPage';
import CustomerPage from './pages/CustomerPage';
import CustomerTrashPage from './pages/CustomerTrashPage';
import BillOfMaterialPage from './pages/BillOfMaterialPage';
import BillOfMaterialTrashPage from './pages/BillOfMaterialTrashPage';
import WorkOrderPage from './pages/WorkOrderPage';
import WorkOrderTrashPage from './pages/WorkOrderTrashPage';
import ProductionConfirmationPage from './pages/ProductionConfirmationPage';
import ProductionConfirmationTrashPage from './pages/ProductionConfirmationTrashPage';
import LocationPage from './pages/LocationPage';
import LocationTrashPage from './pages/LocationTrashPage';
import StockEntryPage from './pages/StockEntryPage';
import StockEntryTrashPage from './pages/StockEntryTrashPage';
import PurchaseOrderPage from './pages/PurchaseOrderPage';
import PurchaseOrderTrashPage from './pages/PurchaseOrderTrashPage';
import SupplierTypePage from './pages/SupplierTypePage';
import SupplierTypeTrashPage from './pages/SupplierTypeTrashPage';
import CustomerOrderPage from './pages/CustomerOrderPage';
import CustomerOrderTrashPage from './pages/CustomerOrderTrashPage';
import ReportsPage from './pages/ReportsPage';

function App() {
  return (
    <AuthProvider>
    <BrowserRouter>
      <RouteScrollToTop />
      <Routes>
          {/* Authentication Routes */}
          <Route 
            path='/sign-in' 
            element={
              <ProtectedRoute requireAuth={false}>
                <SignInPage />
              </ProtectedRoute>
            } 
          />
          {/* Dashboard Routes - Protected */}
        <Route
            path='/' 
            element={
              <ProtectedRoute>
                <HomePageTen />
              </ProtectedRoute>
            } 
          />
          {/* User Management Routes - Admin Only */}
          <Route 
            path='/users-list' 
            element={
              <ProtectedRoute adminOnly={true}>
                <UsersListPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/add-user' 
            element={
              <ProtectedRoute adminOnly={true}>
                <AddUserPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/assign-role' 
            element={
              <ProtectedRoute adminOnly={true}>
                <AssignRolePage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/create-role' 
            element={
              <ProtectedRoute adminOnly={true}>
                <CreateRolePage />
              </ProtectedRoute>
            } 
          />
          {/* Profile Routes - Authenticated Users */}
          <Route 
            path='/view-profile' 
            element={
              <ProtectedRoute>
                <ViewProfilePage />
              </ProtectedRoute>
            } 
          />
          {/* Material Management Routes - Protected */}
          <Route 
            path='/material-cards' 
            element={
              <ProtectedRoute>
                <MaterialCardPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/material-category' 
            element={
              <ProtectedRoute>
                <MaterialCategoryPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/material-movement' 
            element={
              <ProtectedRoute>
                <MaterialMovementPage />
              </ProtectedRoute>
            } 
          />
          {/* Trash Pages */}
          <Route 
            path='/material-card-trash' 
            element={
              <ProtectedRoute>
                <MaterialCardTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/material-category-trash' 
            element={
              <ProtectedRoute>
                <MaterialCategoryTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/material-movement-trash' 
            element={
              <ProtectedRoute>
                <MaterialMovementTrashPage />
              </ProtectedRoute>
            } 
          />
          {/* Warehouse Management Routes - Protected */}
          <Route 
            path='/warehouse' 
            element={
              <ProtectedRoute>
                <WarehousePage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/warehouse-trash' 
            element={
              <ProtectedRoute>
                <WarehouseTrashPage />
              </ProtectedRoute>
            } 
          />
          {/* Purchasing Management Routes - Protected */}
          <Route 
            path='/supplier' 
            element={
              <ProtectedRoute>
                <SupplierPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/supplier-trash' 
            element={
              <ProtectedRoute>
                <SupplierTrashPage />
              </ProtectedRoute>
            } 
          />
          {/* Sales Management Routes - Protected */}
          <Route 
            path='/customer' 
            element={
              <ProtectedRoute>
                <CustomerPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/customer-trash' 
            element={
              <ProtectedRoute>
                <CustomerTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/bill-of-materials' 
            element={
              <ProtectedRoute>
                <BillOfMaterialPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/bill-of-material-trash' 
            element={
              <ProtectedRoute>
                <BillOfMaterialTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/work-orders' 
            element={
              <ProtectedRoute>
                <WorkOrderPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/work-order-trash' 
            element={
              <ProtectedRoute>
                <WorkOrderTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/production-confirmations' 
            element={
              <ProtectedRoute>
                <ProductionConfirmationPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/production-confirmation-trash' 
            element={
              <ProtectedRoute>
                <ProductionConfirmationTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/locations' 
            element={
              <ProtectedRoute>
                <LocationPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/location-trash' 
            element={
              <ProtectedRoute>
                <LocationTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/stock-entries' 
            element={
              <ProtectedRoute>
                <StockEntryPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/stock-entry-trash' 
            element={
              <ProtectedRoute>
                <StockEntryTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/purchase-orders' 
            element={
              <ProtectedRoute>
                <PurchaseOrderPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/purchase-order-trash' 
            element={
              <ProtectedRoute>
                <PurchaseOrderTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/supplier-types' 
            element={
              <ProtectedRoute>
                <SupplierTypePage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/supplier-type-trash' 
            element={
              <ProtectedRoute>
                <SupplierTypeTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/customer-orders' 
            element={
              <ProtectedRoute>
                <CustomerOrderPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/customer-order-trash' 
            element={
              <ProtectedRoute>
                <CustomerOrderTrashPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path='/reports' 
            element={
              <ProtectedRoute>
                <ReportsPage />
              </ProtectedRoute>
            } 
          />
      </Routes>
    </BrowserRouter>
    </AuthProvider>
  );
}

export default App;

